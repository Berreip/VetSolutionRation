using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using PRF.Utils.CoreComponents.Diagnostic;
using PRF.Utils.CoreComponents.Extensions;
using VetSolutionRation.Common.Async;
using VetSolutionRation.DataProvider.Dto;
using VetSolutionRation.wpf.Helpers;
using VetSolutionRation.wpf.Services.Configuration;
using VetSolutionRationLib.Extensions;
using VetSolutionRationLib.Models.Feed;
using VetSolutionRationLib.Models.Recipe;

namespace VetSolutionRation.wpf.Services.Saves;

internal interface IFeedProvider
{
    /// <summary>
    /// Returns all registered feed (either reference or custom feed) or recipe
    /// </summary>
    IEnumerable<IFeedOrRecipe> GetFeedsOrRecipes();

    /// <summary>
    /// Event raised when feeds or recipe change
    /// </summary>
    event Action? OnFeedOrRecipeChanged;

    void AddFeedsAndSave(IReadOnlyCollection<IFeed> newFeeds);
    bool ContainsName(string name);

    /// <summary>
    /// Delete the provided custom feed from reference
    /// </summary>
    void DeleteCustomFeedAndSave(ICustomFeed customFeed);

    void AddRecipeAndSave(IRecipe recipe);
}

public sealed class FeedProvider : IFeedProvider
{
    private readonly object _key = new object();
    private readonly Dictionary<string, IReferenceFeed> _referenceFeedByLabel = new Dictionary<string, IReferenceFeed>(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, ICustomFeed> _customFeedByLabel = new Dictionary<string, ICustomFeed>(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, IRecipe> _recipeByLabel = new Dictionary<string, IRecipe>(StringComparer.OrdinalIgnoreCase);


    private readonly DirectoryInfo _cacheFolder;

    /// <inheritdoc />
    public event Action? OnFeedOrRecipeChanged;

    private readonly string[] _filesToLoad =
    {
        VetSolutionRatioConstants.SAVED_DATA_REFERENCE_FILE_NAME,
        VetSolutionRatioConstants.SAVED_DATA_USER_FILE_NAME,
        VetSolutionRatioConstants.SAVED_RECIPE_USER_FILE_NAME
    };

    public FeedProvider(IConfigurationManager configurationManager)
    {
        _cacheFolder = configurationManager.GetCacheDataFolder();
    }

    public void LoadInitialSavedFeeds()
    {
        if (!_cacheFolder.ExistsExplicit()) return;

        // load every saved data file (reference and user) if they exists
        foreach (var fileName in _filesToLoad)
        {
            try
            {
                if (_cacheFolder.TryGetFile(fileName, out var file))
                {
                    var fileContent = DtoExporter.DeserializeFromJson(file.ReadAllText());
                    if (fileContent.Feeds != null)
                    {
                        // add feeds without saving
                        AddAndSaveIfNeeded(fileContent.Feeds.Select(o => o.ConvertFromDto()).ToArray(), new NoSaveFeedMonitor());
                    }

                    if (fileContent.Recipes != null)
                    {
                        // add feeds without saving
                        foreach (var recipe in fileContent.Recipes)
                        {
                            try
                            {
                                _recipeByLabel.AddOrUpdate(recipe.ConvertFromDto());
                            }
                            catch (Exception e)
                            {
                                ErrorHandler.HandleError($"the recipe : {recipe} could not be loaded and will be ignored. Error: {e}");
                            }
                        }

                        // raise outside the lock
                        RaiseOnFeedOrRecipeChanged();
                    }
                }
            }
            catch (Exception e)
            {
                ErrorHandler.HandleError($"Error while loading the file: {fileName}: {e}");
            }
        }
    }


    /// <inheritdoc />
    public IEnumerable<IFeedOrRecipe> GetFeedsOrRecipes()
    {
        lock (_key)
        {
            var aggregated = new List<IFeedOrRecipe>(_recipeByLabel.Count + _customFeedByLabel.Count + _referenceFeedByLabel.Count);
            aggregated.AddRange(_recipeByLabel.Values);
            aggregated.AddRange(_customFeedByLabel.Values);
            aggregated.AddRange(_referenceFeedByLabel.Values);
            return aggregated;
        }
    }

    public void AddFeedsAndSave(IReadOnlyCollection<IFeed> newFeeds)
    {
        AddAndSaveIfNeeded(newFeeds, new FeedChangeMonitor());
    }

    /// <inheritdoc />
    public void AddRecipeAndSave(IRecipe recipe)
    {
        AddAndSaveIfNeeded(new[] { recipe }, new FeedChangeMonitor());
    }

    /// <inheritdoc />
    public bool ContainsName(string name)
    {
        lock (_key)
        {
            return _customFeedByLabel.ContainsKey(name) || _recipeByLabel.ContainsKey(name);
        }
    }

    /// <inheritdoc />
    public void DeleteCustomFeedAndSave(ICustomFeed customFeed)
    {
        lock (_key)
        {
            if (_customFeedByLabel.TryGetValue(customFeed.Label, out var matchingFeed) && ReferenceEquals(matchingFeed, customFeed))
            {
                _customFeedByLabel.Remove(customFeed.Label);
                SaveCustomFeeds();
            }
        }

        // raise outside the lock
        RaiseOnFeedOrRecipeChanged();
    }


    private void AddAndSaveIfNeeded(IReadOnlyCollection<IFeedOrRecipe> newFeeds, IFeedChangeMonitor monitor)
    {
        if (newFeeds.Count == 0) return;

        lock (_key)
        {

            foreach (var newFeed in newFeeds)
            {
                switch (newFeed)
                {
                    case ICustomFeed customFeed:
                        // if the new one is custom: 2 cases:
                        // 1) if the previous is a reference, ignore it:
                        // 2) if the previous was custom, just replace it
                        if (!_referenceFeedByLabel.ContainsKey(newFeed.UniqueReferenceKey))
                        {
                            _customFeedByLabel.AddOrUpdate(customFeed);
                            monitor.SignalCustomDataChanged();
                        }
                        break;

                    case IReferenceFeed referenceFeed:
                        // if the new one is a reference feed, replace previous whatever it was:
                        // do not allow duplicates in custom feed, so remove it if needed:
                        if (_customFeedByLabel.RemoveIfNeeded(referenceFeed))
                        {
                            monitor.SignalCustomDataChanged();
                        }
                        
                        // then add or update:
                        _referenceFeedByLabel.AddOrUpdate(referenceFeed);
                        monitor.SignalReferenceChanged();
                        break;
                    
                    case IRecipe recipe:
                        // For recipe, just add it to the reference or replace it if already found.
                        _recipeByLabel.AddOrUpdate(recipe);
                        monitor.SignalRecipeChanged();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(newFeed));
                }
            }

            if (monitor.ShouldSaveReference())
            {
                SaveReferenceFeeds();
            }
            if (monitor.ShouldSaveCustomDataChanged())
            {
                SaveCustomFeeds();
            }
            if (monitor.ShouldSaveRecipeChanged())
            {
                SaveRecipe();
            }
        }

        if (monitor.HasAnySave())
        {
            // raise outside the lock
            RaiseOnFeedOrRecipeChanged();
        }
    }
    
    private void SaveRecipe()
    {
        lock (_key)
        {
            SaveShared(_recipeByLabel.Values, VetSolutionRatioConstants.SAVED_RECIPE_USER_FILE_NAME);
        }
    }
    
    private void SaveCustomFeeds()
    {
        lock (_key)
        {
            SaveShared(_customFeedByLabel.Values, VetSolutionRatioConstants.SAVED_DATA_USER_FILE_NAME);
        }
    }

    private void SaveReferenceFeeds()
    {
        lock (_key)
        {
            SaveShared(_referenceFeedByLabel.Values, VetSolutionRatioConstants.SAVED_DATA_REFERENCE_FILE_NAME);
        }
    }

    private void SaveShared(IReadOnlyCollection<IFeedOrRecipe> feeds, string fileName)
    {
        try
        {
            _cacheFolder.CreateIfNotExist();
            var jsonFeeds = feeds.ConvertToDto().SerializeReferenceToJson();
            File.WriteAllText(_cacheFolder.GetFile(fileName).FullName, jsonFeeds);
        }
        catch (Exception e)
        {
            Trace.TraceError($"error while trying to save new feeds: {e}");
            DebugCore.Fail($"error while trying to save new feeds: {e}");
        }
    }

    private void RaiseOnFeedOrRecipeChanged()
    {
        OnFeedOrRecipeChanged?.Invoke();
    }
}