using System.Collections.Generic;
using PRF.WPFCore;
using VetSolutionRation.wpf.Views.Adapter;
using VetSolutionRationLib.Models.Feed;

namespace VetSolutionRation.wpf.Views.Popups.RecipeConfiguration.Adapters;

internal interface IFeedForRecipeCreationAdapter
{
    string Name { get; }
    IFeedQuantityAdapter FeedQuantity { get; }
    IReadOnlyList<(IFeed Feed, double Percentage)> GetUnderlyingFeeds();
}

internal sealed class FeedForRecipeCreationAdapter : ViewModelBase, IFeedForRecipeCreationAdapter
{
    private readonly IVerifyFeed _feed;

    public FeedForRecipeCreationAdapter(IVerifyFeed feed)
    {
        _feed = feed;
        Name = feed.Name;
        // copy the adapter to avoid multipe reference pointing to the same adapter
        FeedQuantity = new FeedQuantityAdapter(feed.FeedQuantity.Unit, feed.FeedQuantity.Quantity);
    }

    public string Name { get; }
    public IFeedQuantityAdapter FeedQuantity { get; }

    public bool IsValidForRecipe()
    {
        // should have a positive quantity
        return FeedQuantity.Quantity > 0d;
    }

    public IReadOnlyList<(IFeed Feed, double Percentage)> GetUnderlyingFeeds()
    {
        // simple case: one unique ingredient
        return new[] { (_feed.GetUnderlyingFeed(), 1d) };
    }
}