using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PRF.WPFCore;
using PRF.WPFCore.CustomCollections;
using VetSolutionRation.wpf.Helpers;
using VetSolutionRation.wpf.Services.Saves;
using VetSolutionRation.wpf.Views.Adapter.Feeds;
using VetSolutionRationLib.Helpers;

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
    private readonly ObservableCollectionRanged<IFeedOrRecipeAdapter> _availableFeeds;
    public ICollectionView AvailableFeeds { get; }

    public FeedProviderHoster(IFeedProvider feedProvider)
    {
        _feedProvider = feedProvider;
        AvailableFeeds = ObservableCollectionSource.GetDefaultView(AggregateDataForAdapters(_feedProvider), out _availableFeeds);
        AvailableFeeds.SortDescriptions.Add(new SortDescription(nameof(IFeedOrRecipeAdapter.IsCustom), ListSortDirection.Descending)); // custom first
        AvailableFeeds.SortDescriptions.Add(new SortDescription(nameof(IFeedOrRecipeAdapter.Name), ListSortDirection.Ascending)); // then in aphabetical order

        feedProvider.OnFeedOrRecipeChanged += OnFeedOrRecipeChanged;
    }

    private void OnFeedOrRecipeChanged()
    {
        _availableFeeds.Reset(AggregateDataForAdapters(_feedProvider));
    }

    private static IEnumerable<IFeedOrRecipeAdapter> AggregateDataForAdapters(IFeedProvider feedProvider)
    {
        return feedProvider.GetFeedsOrRecipes().Select(o => o.CreateAdapter()).ToList();
    }

    private void FilterAvailableFeeds(string? inputText)
    {
        if (inputText == null)
            return;
        var splitByWhitspace = SearchHelpers.SplitByWhitspaceAndSpecificSymbols(inputText);
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