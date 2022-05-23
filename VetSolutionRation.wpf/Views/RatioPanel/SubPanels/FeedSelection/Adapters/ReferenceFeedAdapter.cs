using System;
using VetSolutionRation.wpf.Helpers;
using VetSolutionRation.wpf.Searcheable;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models.Feed;

namespace VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Adapters;

internal interface IFeedWithValue
{
    double GetInraValue(InraHeader inraHeader);
}

internal interface IFeedAdapter : IFeedWithValue
{
    string FeedName { get; }
}

/// <summary>
/// Represent the adapter of a feed that is from reference feed (readonly)
/// </summary>
internal abstract class FeedAdapterBase<T> : SearcheableBase, IFeedAdapter
    where T : IFeed
{
    private readonly T _feed;
    private bool _isSelected;

    protected FeedAdapterBase(T feed) : base(feed.Label)
    {
        _feed = feed;
        FeedName = feed.Label;
    }

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    public string FeedName { get; }

    /// <inheritdoc />
    public double GetInraValue(InraHeader inraHeader)
    {
        return _feed.TryGetInraValue(inraHeader, out var matchingValue) ? matchingValue : VetSolutionRatioConstants.DEFAULT_FEED_VALUE;
    }
}

internal interface IReferenceFeedAdapter : IFeedAdapter
{
}

/// <summary>
/// Represent the adapter of a feed that is from reference feed (readonly)
/// </summary>
internal sealed class ReferenceFeedAdapter : FeedAdapterBase<IReferenceFeed>, IReferenceFeedAdapter
{
    public ReferenceFeedAdapter(IReferenceFeed feed) : base(feed)
    {
    }
}

/// <summary>
/// Represent the adapter of a feed that has been created by user
/// </summary>
internal sealed class CustomUserFeedAdapter : FeedAdapterBase<ICustomFeed>
{
    public CustomUserFeedAdapter(ICustomFeed feed) : base(feed)
    {
    }
}

internal static class FeedAdapterExtensions
{
    public static IFeedAdapter CreateAdapter(this IFeed feed)
    {
        switch (feed)
        {
            case ICustomFeed customFeed:
                return new CustomUserFeedAdapter(customFeed);
            case IReferenceFeed referenceFeed:
                return new ReferenceFeedAdapter(referenceFeed);
            default:
                throw new ArgumentOutOfRangeException(nameof(feed));
        }
    }
}