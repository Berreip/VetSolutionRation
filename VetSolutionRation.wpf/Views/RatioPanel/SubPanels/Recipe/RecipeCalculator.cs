﻿using System.Collections.Generic;
using VetSolutionRation.wpf.Views.Popups.RecipeConfiguration;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Adapters;
using VetSolutionRationLib.Models.Feed;
using VetSolutionRationLib.Models.Recipe;
using System.Linq;

namespace VetSolutionRation.wpf.Views.RatioPanel.SubPanels.Recipe;

/// <summary>
/// Represent a feed that could be aded into a reciepe
/// </summary>
internal interface IFeedThatCouldBeAddedIntoReciepe
{
    bool IsSelected { get; }
    string Name { get; }
    IFeedQuantityAdapter FeedQuantity { get; }
}

internal interface IRecipeCalculator
{
    /// <summary>
    /// Returns true if the given list of feed could create a reciepe
    /// </summary>
    bool CouldCalculateRecipe(IReadOnlyCollection<IFeedThatCouldBeAddedIntoReciepe> feeds);
    IRecipe CreateNewReciepe(IRecipeConfiguration recipeConfiguration);
}


internal sealed class RecipeCalculator : IRecipeCalculator
{
    /// <inheritdoc />
    public bool CouldCalculateRecipe(IReadOnlyCollection<IFeedThatCouldBeAddedIntoReciepe> feeds)
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