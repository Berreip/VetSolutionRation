using System;
using System.Collections.Generic;
using System.Linq;
using VetSolutionRation.wpf.Helpers;

namespace VetSolutionRation.wpf.Services.Feed;

internal interface IFeedProvider
{
    IEnumerable<string> GetLabels();
    void LoadLabels(FileFeedSource fileFeedSource, IEnumerable<string> labels);
    event Action OnNewDataProvided;
}

public sealed class FeedProvider : IFeedProvider
{
    private readonly object _key = new object();
    private readonly Dictionary<FileFeedSource, IReadOnlyCollection<string>> _labelsBySource = new Dictionary<FileFeedSource, IReadOnlyCollection<string>>();

    /// <inheritdoc />
    public IEnumerable<string> GetLabels()
    {
        lock (_key)
        {
            var labels = new List<string>();
            foreach (var label in _labelsBySource)
            {
                labels.AddRange(label.Value);
            }

            return labels;
        }
    }

    public void LoadLabels(FileFeedSource fileFeedSource, IEnumerable<string> labels)
    {
        lock (_key)
        {
            if (_labelsBySource.ContainsKey(fileFeedSource))
            {
                _labelsBySource[fileFeedSource] = labels.Select(o => o.ToLower()).ToArray();
            }
            else
            {
                _labelsBySource.Add(fileFeedSource, labels.Select(o => o.ToLower()).ToArray());
            }

            RaiseOnNewDataProvided();
        }
    }

    /// <inheritdoc />
    public event Action? OnNewDataProvided;

    private void RaiseOnNewDataProvided()
    {
        OnNewDataProvided?.Invoke();
    }
}