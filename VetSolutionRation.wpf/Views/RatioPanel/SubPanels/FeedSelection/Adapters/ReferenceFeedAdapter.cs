using System;
using PRF.WPFCore;
using VetSolutionRation.wpf.Searcheable;
using VetSolutionRationLib.Models.Feed;

namespace VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Adapters;

internal interface IFeedAdapter
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
}

/// <summary>
/// Represent the adapter of a feed that is from reference feed (readonly)
/// </summary>
internal sealed class ReferenceFeedAdapter : FeedAdapterBase
{
    public ReferenceFeedAdapter(IReferenceFeed feed) : base(feed.Label)
    {
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