using VetSolutionRation.wpf.Helpers;
using VetSolutionRation.wpf.Helpers.Searcheable;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models.Feed;

namespace VetSolutionRation.wpf.Views.Adapter.Feeds;

internal interface IFeedWithValue
{
    double GetInraValue(InraHeader inraHeader);
}

internal interface IFeedAdapter : IFeedWithValue, IFeedOrRecipeAdapter
{
    IFeed GetUnderlyingFeed();
}

/// <summary>
/// Represent the adapter of a feed that is from reference feed (readonly)
/// </summary>
internal abstract class FeedAdapterBase<T> : SearcheableBase, IFeedAdapter
    where T : IFeed
{
    public bool IsCustom { get; }
    protected readonly T _feed;
    private bool _isSelected;

    protected FeedAdapterBase(T feed, bool isCustom) : base(feed.Label)
    {
        IsCustom = isCustom;
        _feed = feed;
        Name = feed.Label;
    }

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    public string Name { get; }

    /// <inheritdoc />
    public double GetInraValue(InraHeader inraHeader)
    {
        return _feed.TryGetInraValue(inraHeader, out var matchingValue) ? matchingValue : VetSolutionRatioConstants.DEFAULT_FEED_VALUE;
    }

    public IFeed GetUnderlyingFeed() => _feed;
}