using System.Collections.Generic;
using System.Linq;
using PRF.WPFCore;
using VetSolutionRation.wpf.Views.RatioPanel.Recipe;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models.Feed;
using VetSolutionRationLib.Models.Recipe;

namespace VetSolutionRation.wpf.Views.Adapter;

internal interface IRecipeAdapter : IFeedThatCouldBeAddedIntoRecipe
{
    /// <summary>
    /// list of feed ingredients
    /// </summary>
    IReadOnlyList<IVerifyFeed> Ingredients { get; }
}

internal sealed class RecipeAdapter : ViewModelBase, IRecipeAdapter
{
    private bool _isSelected = true;

    public RecipeAdapter(IRecipe recipe)
    {
        Name = recipe.RecipeName;
        FeedQuantity = new FeedQuantityAdapter(recipe.Unit);
        Ingredients = recipe.Ingredients.Select(o => new IngredientFeedAdapter(o.Ingredient)).ToArray();
    }

    public IReadOnlyList<IVerifyFeed> Ingredients { get; }

    /// <inheritdoc />
    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public IFeedQuantityAdapter FeedQuantity { get; }

    /// <summary>
    /// Adapter of an ingredient
    /// </summary>
    private sealed class IngredientFeedAdapter : IVerifyFeed 
    {
        private readonly IFeed _feed;

        public IngredientFeedAdapter(IFeed feed)
        {
            _feed = feed;
            Name = feed.Label;
            IsSelected = true;
            FeedQuantity = new FeedQuantityAdapter(FeedUnit.Kg);
        }

        /// <inheritdoc />
        public bool IsSelected { get; }

        /// <inheritdoc />
        public string Name { get; }

        /// <inheritdoc />
        public IFeedQuantityAdapter FeedQuantity { get; }

        /// <inheritdoc />
        public IFeed GetUnderlyingFeed() => _feed;
    }

}