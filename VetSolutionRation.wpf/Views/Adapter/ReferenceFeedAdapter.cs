using System;
using VetSolutionRation.wpf.Helpers;
using VetSolutionRation.wpf.Helpers.Searcheable;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models.Feed;

namespace VetSolutionRation.wpf.Views.Adapter;

/// <summary>
/// Represent either a feed or a recipe
/// </summary>
internal interface IFeedOrReciepe
{
}

internal interface IFeedWithValue : IFeedOrReciepe
{
    double GetInraValue(InraHeader inraHeader);
}

internal interface IFeedAdapter : IFeedWithValue
{
    string FeedName { get; }
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
    
    public IFeed GetUnderlyingFeed() => _feed;
}

internal interface IReferenceFeedAdapter : IFeedAdapter
{
}

/// <summary>
/// Represent the adapter of a feed that is from reference feed (readonly)
/// </summary>
internal sealed class ReferenceFeedAdapter : FeedAdapterBase<IReferenceFeed>, IReferenceFeedAdapter
{
    public ReferenceFeedAdapter(IReferenceFeed feed) : base(feed, false)
    {
    }
}

/// <summary>
/// Represent the adapter of a feed that has been created by user
/// </summary>
internal sealed class CustomUserFeedAdapter : FeedAdapterBase<ICustomFeed>
{
    public CustomUserFeedAdapter(ICustomFeed feed) : base(feed, true)
    {
    }

    public ICustomFeed GetUnderlyingCustomFeed() => _feed;
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