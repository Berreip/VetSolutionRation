using System;
using System.Collections.Generic;
using System.Linq;
using PRF.WPFCore;
using VetSolutionRation.wpf.Views.Adapter.Feeds;
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
    IReadOnlyList<IIngredientFeedAdapter> Ingredients { get; }
}

internal interface IIngredientFeedAdapter : IVerifyFeed
{
    IIngredientForRecipe GetFeedForRecipe();
}

internal sealed class RecipeAdapter : ViewModelBase, IRecipeAdapter, IFeedOrRecipeAdapter
{
    private bool _isSelected = true;

    public RecipeAdapter(IRecipe recipe)
    {
        Name = recipe.RecipeName;
        FeedQuantity = new FeedQuantityAdapter(recipe.Unit);
        Ingredients = recipe.Ingredients.Select(o => new IngredientFeedAdapter(o.Ingredient)).ToArray();
    }

    public IFeedOrRecipe GetUnderlyingRecipeModel()
    {
        // TODO PBO => quel pourcentage prendre? => décider et externaliser dans calculateur
        return new RecipeModel(Name, FeedUnit.Kg, Ingredients.Select(o => o.GetFeedForRecipe()).ToArray());
    }
    
    public IReadOnlyList<IIngredientFeedAdapter> Ingredients { get; }

    /// <inheritdoc />
    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    public string Name { get; }

    /// <inheritdoc />
    public bool IsCustom { get; } = true;

    /// <inheritdoc />
    public IFeedQuantityAdapter FeedQuantity { get; }

    /// <summary>
    /// Adapter of an ingredient
    /// </summary>
    private sealed class IngredientFeedAdapter : IIngredientFeedAdapter
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

        /// <inheritdoc />
        public Guid Guid => _feed.Guid;

        /// <inheritdoc />
        public IIngredientForRecipe GetFeedForRecipe()
        {
            return new IngredientForRecipe(0, _feed);
        }
    }
}