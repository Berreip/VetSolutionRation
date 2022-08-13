using VetSolutionRationLib.Models.Feed;

namespace VetSolutionRation.wpf.Views.Adapter.Feeds;

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