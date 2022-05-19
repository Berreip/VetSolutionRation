using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using PRF.Utils.CoreComponents.Diagnostic;
using PRF.Utils.CoreComponents.Extensions;
using VetSolutionRation.Common.Async;
using VetSolutionRation.DataProvider.Dto;
using VetSolutionRation.wpf.Helpers;
using VetSolutionRation.wpf.Services.Configuration;
using VetSolutionRationLib.Models.Feed;

namespace VetSolutionRation.wpf.Services.Feed;

internal interface IFeedProvider
{
    IEnumerable<IFeed> GetFeeds();
    event Action OnNewDataProvided;
    void AddFeedsAndSave(IReadOnlyCollection<IFeed> newFeeds);
}

public sealed class FeedProvider : IFeedProvider
{
    private readonly IConfigurationManager _configurationManager;
    private readonly object _key = new object();
    private readonly Dictionary<string, IFeed> _feedByLabels = new Dictionary<string, IFeed>(StringComparer.OrdinalIgnoreCase);
    private readonly DirectoryInfo _cacheFolder;
    private readonly string[] _filesToLoad = { VetSolutionRatioConstants.SAVED_DATA_REFERENCE_FILE_NAME, VetSolutionRatioConstants.SAVED_DATA_USER_FILE_NAME };

    public FeedProvider(IConfigurationManager configurationManager)
    {
        _configurationManager = configurationManager;
        _cacheFolder = _configurationManager.GetCacheDataFolder();
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
        }
        // raise outside the lock
        RaiseOnNewDataProvided();
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
    public event Action? OnNewDataProvided;

    private void RaiseOnNewDataProvided()
    {
        OnNewDataProvided?.Invoke();
    }
}