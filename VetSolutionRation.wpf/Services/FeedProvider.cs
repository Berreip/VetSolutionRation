using System.ComponentModel;
using System.Linq;
using PRF.WPFCore.CustomCollections;
using VetSolutionRation.wpf.Helpers;
using VetSolutionRation.wpf.Searcheable;
using VetSolutionRation.wpf.Services.Feed;
using VetSolutionRationLib.Helpers;

namespace VetSolutionRation.wpf.Services;

internal interface IFeedProviderHoster
{
    ICollectionView AvailableFeedForVerify { get; }
    void FilterAvailableFeedForVerify(string? inputText);
}

internal sealed class FeedProviderHoster : IFeedProviderHoster
{
    private readonly IFeedProvider _feedProvider;
    private readonly ObservableCollectionRanged<FeedVerificationAdapter> _availableFeedForVerify;
    public ICollectionView AvailableFeedForVerify { get; }

    public FeedProviderHoster(IFeedProvider feedProvider)
    {
        _feedProvider = feedProvider;
        AvailableFeedForVerify = ObservableCollectionSource.GetDefaultView(feedProvider.GetLabels().Select(o => new FeedVerificationAdapter(o)), out _availableFeedForVerify);
        feedProvider.OnNewDataProvided += OnNewDataProvided;
    }

    private void OnNewDataProvided()
    {
        _availableFeedForVerify.Reset(_feedProvider.GetLabels().Select(o => new FeedVerificationAdapter(o)));
    }

    public void FilterAvailableFeedForVerify(string? inputText)
    {
        if (inputText == null)
            return;
        var splitByWhitspace = SearchHelpers.SplitByWhitspace(inputText);
        AvailableFeedForVerify.Filter = item => SearchFilters.FilterParts(item, splitByWhitspace);
    }
}

internal sealed class FeedVerificationAdapter : SearcheableBase
{
    private readonly string _inputText;

    public FeedVerificationAdapter(string inputText) : base(inputText)
    {
        _inputText = inputText;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return _inputText;
    }
}