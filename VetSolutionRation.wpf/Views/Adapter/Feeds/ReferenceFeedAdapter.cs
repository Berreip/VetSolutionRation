using System;
using VetSolutionRationLib.Models.Feed;
using VetSolutionRationLib.Models.Recipe;

namespace VetSolutionRation.wpf.Views.Adapter.Feeds;

/// <summary>
/// Signal an adapter of either a feed or a recipe
/// </summary>
internal interface IFeedOrRecipeAdapter
{
    public string Name { get; }

    bool IsCustom { get; }
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

internal static class FeedAdapterExtensions
{
    public static IFeedOrRecipeAdapter CreateAdapter(this IFeedOrRecipe feed)
    {
        switch (feed)
        {
            case ICustomFeed customFeed:
                return new CustomUserFeedAdapter(customFeed);
            case IReferenceFeed referenceFeed:
                return new ReferenceFeedAdapter(referenceFeed);
            case IRecipe recipe:
                return new RecipeAdapter(recipe);
            default:
                throw new ArgumentOutOfRangeException(nameof(feed));
        }
    }
}