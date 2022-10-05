using System;
using System.Collections.Generic;
using System.Linq;
using VetSolutionRation.wpf.Views.RecipeConfiguration;
using VSR.Models.Recipe;
using VSR.WPF.Utils.Adapters.CalculationAdapters;

namespace VetSolutionRation.wpf.Views;

internal interface IRecipeCalculator
{
    /// <summary>
    /// Returns true if the given list of feed could create a reciepe
    /// </summary>
    bool CouldCalculateRecipe(IReadOnlyCollection<IAdapterInCalculation> feeds);
    IRecipeCandidate CreateNewRecipe(IRecipeConfiguration recipeConfiguration);
}


internal sealed class RecipeCalculator : IRecipeCalculator
{
    /// <inheritdoc />
    public bool CouldCalculateRecipe(IReadOnlyCollection<IAdapterInCalculation> feeds)
    {
        // doing a recipe with one ingredient just pollute the database
        return feeds.Count(o => o.IsSelected) > 1;
    }

    /// <inheritdoc />
    public IRecipeCandidate CreateNewRecipe(IRecipeConfiguration recipeConfiguration)
    {
        return new RecipeCandidate(
            Guid.NewGuid(), 
            recipeConfiguration.RecipeName,
            recipeConfiguration.GetIngredients().Select(o => new IngredientForRecipeCandidate(o.Percentage, o.Ingredient.Guid)).ToArray());
    }
}