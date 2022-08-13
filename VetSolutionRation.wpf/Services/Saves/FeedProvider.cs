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
using VetSolutionRationLib.Models.Feed;
using VetSolutionRationLib.Models.Recipe;

namespace VetSolutionRation.wpf.Services.Saves;

internal interface IFeedProvider
{
    /// <summary>
    /// Returns all registered recipes
    /// </summary>
    IEnumerable<IRecipe> GetRecipe();

    /// <summary>
    /// Returns all registered feed (either reference or custom feed)
    /// </summary>
    IEnumerable<IFeed> GetFeeds();

    /// <summary>
    /// Event raised when feeds reference change
    /// </summary>
    event Action? OnFeedChanged;

    /// <summary>
    /// Event raised when recipe reference change
    /// </summary>
    event Action? OnRecipeChanged;

    void AddFeedsAndSave(IReadOnlyCollection<IFeed> newFeeds);
    bool ContainsFeedName(string feedEditedName);

    /// <summary>
    /// Delete the provided custom feed from reference
    /// </summary>
    void DeleteFeedAndSave(ICustomFeed customFeed);

    void AddRecipeAndSave(IRecipe recipe);
    bool ContainsRecipeName(string recipeName);
}

public sealed class FeedProvider : IFeedProvider
{
    private readonly object _key = new object();
    private readonly Dictionary<string, IFeed> _feedByLabels = new Dictionary<string, IFeed>(StringComparer.OrdinalIgnoreCase);
    private readonly Dictionary<string, IRecipe> _recipeByLabels = new Dictionary<string, IRecipe>(StringComparer.OrdinalIgnoreCase);
    private readonly DirectoryInfo _cacheFolder;

    /// <inheritdoc />
    public event Action? OnFeedChanged;

    /// <inheritdoc />
    public event Action? OnRecipeChanged;

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
                        AddFeedsAndSaveIfNeeded(fileContent.Feeds.Select(o => o.ConvertFromDto()).ToArray(), false);
                    }

                    if (fileContent.Recipes != null)
                    {
                        // add feeds without saving
                        foreach (var recipe in fileContent.Recipes)
                        {
                            try
                            {
                                AddOrUpdateRecipe(recipe.ConvertFromDto());
                            }
                            catch (Exception e)
                            {
                                ErrorHandler.HandleError($"the recipe : {recipe} could not be loaded and will be ignored. Error: {e}");
                            }
                        }

                        // raise outside the lock
                        RaiseOnRecipeChanged();
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
    public IEnumerable<IFeed> GetFeeds()
    {
        lock (_key)
        {
            return _feedByLabels.Values.ToArray();
        }
    }

    /// <inheritdoc />
    public IEnumerable<IRecipe> GetRecipe()
    {
        lock (_key)
        {
            return _recipeByLabels.Values.ToArray();
        }
    }

    public void AddFeedsAndSave(IReadOnlyCollection<IFeed> newFeeds)
    {
        AddFeedsAndSaveIfNeeded(newFeeds, true);
    }

    /// <inheritdoc />
    public void AddRecipeAndSave(IRecipe recipe)
    {
        lock (_key)
        {
            AddOrUpdateRecipe(recipe);
            SaveRecipe();
        }

        // raise outside the lock
        RaiseOnRecipeChanged();
    }

    private void AddOrUpdateRecipe(IRecipe recipe)
    {
        if (_recipeByLabels.ContainsKey(recipe.RecipeName))
        {
            // The recipe has already been registered and will be overriden
            _recipeByLabels[recipe.RecipeName] = recipe;
        }
        else
        {
            _recipeByLabels.Add(recipe.RecipeName, recipe);
        }
    }

    /// <inheritdoc />
    public bool ContainsFeedName(string feedEditedName)
    {
        lock (_key)
        {
            return _feedByLabels.ContainsKey(feedEditedName);
        }
    }

    /// <inheritdoc />
    public bool ContainsRecipeName(string recipeName)
    {
        lock (_key)
        {
            return _recipeByLabels.ContainsKey(recipeName);
        }
    }

    /// <inheritdoc />
    public void DeleteFeedAndSave(ICustomFeed customFeed)
    {
        lock (_key)
        {
            if (_feedByLabels.TryGetValue(customFeed.Label, out var matchingFeed) && ReferenceEquals(matchingFeed, customFeed))
            {
                _feedByLabels.Remove(customFeed.Label);
                Save(true, false);
            }
        }

        // raise outside the lock
        RaiseOnFeedChanged();
    }


    private void AddFeedsAndSaveIfNeeded(IReadOnlyCollection<IFeed> newFeeds, bool shouldSave)
    {
        if (newFeeds.Count == 0) return;

        lock (_key)
        {
            var referenceChanged = false;
            var userDataChanged = false;

            foreach (var newFeed in newFeeds)
            {
                if (_feedByLabels.TryGetValue(newFeed.Label, out var previous))
                {
                    // if the new one is a reference feed, replace previous 
                    if (newFeed is IReferenceFeed)
                    {
                        _feedByLabels[newFeed.Label] = newFeed;
                        referenceChanged = true;
                    }

                    // else if previous is custom and new one is custom replace it
                    if (previous is ICustomFeed)
                    {
                        _feedByLabels[newFeed.Label] = newFeed;
                        userDataChanged = true;
                    }
                    // else if the previous is a reference feed, ignore the custom new one
                }
                else
                {
                    // if not there, juste add it
                    _feedByLabels.Add(newFeed.Label, newFeed);
                    if (newFeed is IReferenceFeed)
                    {
                        referenceChanged = true;
                    }
                    else
                    {
                        userDataChanged = true;
                    }
                }
            }

            if (shouldSave && (referenceChanged || userDataChanged))
            {
                Save(userDataChanged, referenceChanged);
            }
        }

        // raise outside the lock
        RaiseOnFeedChanged();
    }

    private void Save(bool userDataChanged, bool referenceChanged)
    {
        var referenceFeeds = new List<IFeed>(_feedByLabels.Count);
        var customFeeds = new List<IFeed>();
        foreach (var feedByLabel in _feedByLabels)
        {
            switch (feedByLabel.Value)
            {
                case ICustomFeed customFeed:
                    customFeeds.Add(customFeed);
                    break;
                case IReferenceFeed referenceFeed:
                    referenceFeeds.Add(referenceFeed);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(feedByLabel.Value));
            }
        }

        if (userDataChanged)
        {
            SaveFeeds(customFeeds, VetSolutionRatioConstants.SAVED_DATA_USER_FILE_NAME);
        }

        if (referenceChanged)
        {
            SaveFeeds(referenceFeeds, VetSolutionRatioConstants.SAVED_DATA_REFERENCE_FILE_NAME);
        }
    }

    private void SaveFeeds(IReadOnlyCollection<IFeed> feeds, string fileName)
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

    private void SaveRecipe()
    {
        try
        {
            _cacheFolder.CreateIfNotExist();
            var json = _recipeByLabels.Values.ConvertToDto().SerializeReferenceToJson();
            File.WriteAllText(_cacheFolder.GetFile(VetSolutionRatioConstants.SAVED_RECIPE_USER_FILE_NAME).FullName, json);
        }
        catch (Exception e)
        {
            Trace.TraceError($"error while trying to save new feeds: {e}");
            DebugCore.Fail($"error while trying to save new feeds: {e}");
        }
    }

    private void RaiseOnRecipeChanged()
    {
        OnRecipeChanged?.Invoke();
    }

    private void RaiseOnFeedChanged()
    {
        OnFeedChanged?.Invoke();
    }
}