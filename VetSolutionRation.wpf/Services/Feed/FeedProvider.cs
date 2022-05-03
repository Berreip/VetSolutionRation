using System;
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
    void AddFeeds(IEnumerable<IFeed> newFeeds);
}

public sealed class FeedProvider : IFeedProvider
{
    private readonly IConfigurationManager _configurationManager;
    private readonly object _key = new object();
    private readonly Dictionary<string, IFeed> _feedByLabels = new Dictionary<string, IFeed>(StringComparer.OrdinalIgnoreCase);
    private readonly DirectoryInfo _cacheFolder;

    public FeedProvider(IConfigurationManager configurationManager)
    {
        _configurationManager = configurationManager;
        _cacheFolder = _configurationManager.GetCacheDataFolder();
    }

    public void LoadInitialSavedFeeds()
    {
        AsyncWrapper.DispatchAndWrapInFireAndForget(() =>
        {
            if (_cacheFolder.ExistsExplicit() && _cacheFolder.TryGetFile(VetSolutionRatioConstants.SAVED_DATA_FILE_NAME, out var file))
            {
                var fileContent = DtoExporter.DeserializeFromJson(file.ReadAllText());
                if (fileContent.Feeds != null)
                {
                    AddFeeds(fileContent.Feeds.Select(o => o.ConvertFromDto()));
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

    public void AddFeeds(IEnumerable<IFeed> newFeeds)
    {
        lock (_key)
        {
            foreach (var newFeed in newFeeds)
            {
                if (_feedByLabels.TryGetValue(newFeed.Label, out var previous))
                {
                    // if the new one is a reference feed, replace previous 
                    if (newFeed is IReferenceFeed)
                    {
                        _feedByLabels[newFeed.Label] = newFeed;
                    }
                    // else if previous is custom and new one is cutom replace it
                    if (previous is ICustomFeed)
                    {
                        _feedByLabels[newFeed.Label] = newFeed;
                    }
                    // else if the previous is a reference feed, ignore the custom new one
                }
                else
                {
                    // if not there, juste add it
                    _feedByLabels.Add(newFeed.Label, newFeed);
                }
            }

            SaveFeeds(_feedByLabels.Values.ToArray());
        }
        // raise outside the lock
        RaiseOnNewDataProvided();
    }

    private void SaveFeeds(IFeed[] feeds)
    {
        try
        {
            _cacheFolder.CreateIfNotExist();
            var jsonFeeds = feeds.ConvertToDto().SerializeReferenceToJson();
            File.WriteAllText(_cacheFolder.GetFile(VetSolutionRatioConstants.SAVED_DATA_FILE_NAME).FullName, jsonFeeds);
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