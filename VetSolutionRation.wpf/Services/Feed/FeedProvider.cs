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

namespace VetSolutionRation.wpf.Services.Feed;

internal interface IFeedProvider
{
    IEnumerable<IFeed> GetFeeds();
    event Action OnFeedChanged;
    void AddFeedsAndSave(IReadOnlyCollection<IFeed> newFeeds);
    bool ContainsFeedName(string feedEditedName);
    
    /// <summary>
    /// Delete the provided custom feed from reference
    /// </summary>
    void DeleteFeedAndSave(ICustomFeed customFeed);

    void SaveReciepe(IRecipe recipe);
    bool ContainsRecipeName(string reciepeName);
}

public sealed class FeedProvider : IFeedProvider
{
    private readonly object _key = new object();
    private readonly Dictionary<string, IFeed> _feedByLabels = new Dictionary<string, IFeed>(StringComparer.OrdinalIgnoreCase);
    private readonly DirectoryInfo _cacheFolder;
    private readonly string[] _filesToLoad = { VetSolutionRatioConstants.SAVED_DATA_REFERENCE_FILE_NAME, VetSolutionRatioConstants.SAVED_DATA_USER_FILE_NAME };

    public FeedProvider(IConfigurationManager configurationManager)
    {
        _cacheFolder = configurationManager.GetCacheDataFolder();
    }

    public void LoadInitialSavedFeeds()
    {
        AsyncWrapper.DispatchAndWrapInFireAndForget(() =>
        {
            if (!_cacheFolder.ExistsExplicit()) return;
            
            // load every saved data file (reference and user) if they exists
            foreach (var fileName in _filesToLoad)
            {
                if (_cacheFolder.TryGetFile(fileName, out var file))
                {
                    var fileContent = DtoExporter.DeserializeFromJson(file.ReadAllText());
                    if (fileContent.Feeds != null)
                    {
                        // add feeds without saving
                        AddFeedsAndSaveIfNeeded(fileContent.Feeds.Select(o => o.ConvertFromDto()).ToArray(), false);
                    }
                }  
            }
        });
    }


    /// <inheritdoc />
    public IEnumerable<IFeed> GetFeeds()
    {
        lock (_key)
        {
            return _feedByLabels.Values.ToArray();
        }
    }

    public void AddFeedsAndSave(IReadOnlyCollection<IFeed> newFeeds)
    {
        AddFeedsAndSaveIfNeeded(newFeeds, true);
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

    /// <inheritdoc />
    public void SaveReciepe(IRecipe recipe)
    {
        // TODO PBO
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public bool ContainsRecipeName(string reciepeName)
    {
        throw new NotImplementedException();
    }

    private void AddFeedsAndSaveIfNeeded(IReadOnlyCollection<IFeed> newFeeds, bool shouldSave)
    {
        if(newFeeds.Count == 0) return;
        
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
        if (feeds.Count == 0)
        {
            return;
        }
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

    /// <inheritdoc />
    public event Action? OnFeedChanged;

    private void RaiseOnFeedChanged()
    {
        OnFeedChanged?.Invoke();
    }
}