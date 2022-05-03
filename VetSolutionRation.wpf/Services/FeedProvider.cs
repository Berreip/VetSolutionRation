using System;
using System.ComponentModel;
using System.Linq;
using PRF.WPFCore;
using PRF.WPFCore.CustomCollections;
using VetSolutionRation.wpf.Helpers;
using VetSolutionRation.wpf.Services.Feed;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Adapters;
using VetSolutionRationLib.Helpers;
using VetSolutionRationLib.Models.Feed;

namespace VetSolutionRation.wpf.Services;

internal interface IFeedProviderHoster
{
    ICollectionView AvailableFeeds { get; }
    string? SearchFilter { get; set; }
}

internal sealed class FeedProviderHoster : ViewModelBase, IFeedProviderHoster
{
    private readonly IFeedProvider _feedProvider;
    private string? _searchText;
    private readonly ObservableCollectionRanged<IFeedAdapter> _availableFeeds;
    public ICollectionView AvailableFeeds { get; }

    public FeedProviderHoster(IFeedProvider feedProvider)
    {
        _feedProvider = feedProvider;
        AvailableFeeds = ObservableCollectionSource.GetDefaultView(feedProvider.GetFeeds().Select(CreateAdapter), out _availableFeeds);
        AvailableFeeds.SortDescriptions.Add(new SortDescription(nameof(ReferenceFeedAdapter.FeedName), ListSortDirection.Ascending));
       
        feedProvider.OnNewDataProvided += OnNewDataProvided;
    }

    private static IFeedAdapter CreateAdapter(IFeed o)
    {
        switch (o)
        {
            case ICustomFeed customFeed:
                return new CustomUserFeedAdapter(customFeed);
            case IReferenceFeed referenceFeed:
                return new ReferenceFeedAdapter(referenceFeed);
            default:
                throw new ArgumentOutOfRangeException(nameof(o));
        }
    }

    private void OnNewDataProvided()
    {
        _availableFeeds.Reset(_feedProvider.GetFeeds().Select(CreateAdapter));
    }

    public void FilterAvailableFeeds(string? inputText)
    {
        if (inputText == null)
            return;
        var splitByWhitspace = SearchHelpers.SplitByWhitspace(inputText);
        AvailableFeeds.Filter = item => SearchFilters.FilterParts(item, splitByWhitspace);
    }
    
    public string? SearchFilter
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
            {
                FilterAvailableFeeds(value);
            }
        }
    }
}