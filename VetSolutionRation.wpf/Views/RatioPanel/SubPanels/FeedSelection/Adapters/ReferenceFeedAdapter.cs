using System;
using PRF.WPFCore;
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
}


/// <summary>
/// Represent the adapter of a feed that is from reference feed (readonly)
/// </summary>
internal abstract class FeedAdapterBase : SearcheableBase, IFeedAdapter
{
    private bool _isSelected;

    protected FeedAdapterBase(string feedName) : base(feedName)
    {
        FeedName = feedName;
    }

    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    public string FeedName { get; }

    public abstract double GetInraValue(InraHeader inraHeader);
}

/// <summary>
/// Represent the adapter of a feed that is from reference feed (readonly)
/// </summary>
internal sealed class ReferenceFeedAdapter : FeedAdapterBase
{
    private readonly IReferenceFeed _feed;

    public ReferenceFeedAdapter(IReferenceFeed feed) : base(feed.Label)
    {
        _feed = feed;
    }

    /// <inheritdoc />
    public override double GetInraValue(InraHeader inraHeader)
    {
        return _feed.TryGetInraValue(inraHeader, out var matchingValue) ? matchingValue : VetSolutionRatioConstants.DEFAULT_FEED_VALUE;
    }
}

/// <summary>
/// Represent the adapter of a feed that has been created by user
/// </summary>
internal sealed class CustomUserFeedAdapter : FeedAdapterBase
{
    public CustomUserFeedAdapter(ICustomFeed feed) : base(feed.Label)
    {
    }

    /// <inheritdoc />
    public override double GetInraValue(InraHeader inraHeader)
    {
        // TODO PBO
        throw new NotImplementedException();
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