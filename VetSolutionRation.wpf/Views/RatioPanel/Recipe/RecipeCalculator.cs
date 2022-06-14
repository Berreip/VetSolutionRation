using System.Collections.Generic;
using System.Linq;
using VetSolutionRation.wpf.Views.Adapter;
using VetSolutionRation.wpf.Views.Popups.RecipeConfiguration;
using VetSolutionRationLib.Models.Recipe;

namespace VetSolutionRation.wpf.Views.RatioPanel.Recipe;

/// <summary>
/// Represent a feed that could be aded into a recipe
/// <remarks>It could be a recipe itself</remarks>
/// </summary>
internal interface IFeedThatCouldBeAddedIntoRecipe
{
    string Name { get; }
    IFeedQuantityAdapter FeedQuantity { get; }
    bool IsSelected { get; }
}

internal interface IRecipeCalculator
{
    /// <summary>
    /// Returns true if the given list of feed could create a reciepe
    /// </summary>
    bool CouldCalculateRecipe(IReadOnlyCollection<IFeedThatCouldBeAddedIntoRecipe> feeds);
    IRecipe CreateNewReciepe(IRecipeConfiguration recipeConfiguration);
}


internal sealed class RecipeCalculator : IRecipeCalculator
{
    /// <inheritdoc />
    public bool CouldCalculateRecipe(IReadOnlyCollection<IFeedThatCouldBeAddedIntoRecipe> feeds)
    {
        // doing a recipe with one ingredient just pollute the database
        return feeds.Count(o => o.IsSelected) > 1;
    }

    /// <inheritdoc />
    public IRecipe CreateNewReciepe(IRecipeConfiguration recipeConfiguration)
    {
        return new RecipeModel(recipeConfiguration.RecipeName, recipeConfiguration.RecipeUnit, recipeConfiguration.GetIngredients());
    }
}