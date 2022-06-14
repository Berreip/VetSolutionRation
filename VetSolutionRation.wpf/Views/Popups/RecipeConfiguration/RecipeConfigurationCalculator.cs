using System;
using System.Collections.Generic;
using VetSolutionRation.wpf.Helpers;
using VetSolutionRation.wpf.Views.Adapter;
using VetSolutionRation.wpf.Views.RatioPanel.Recipe;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models.Feed;
using VetSolutionRationLib.Models.Recipe;

namespace VetSolutionRation.wpf.Views.Popups.RecipeConfiguration;

internal static class RecipeConfigurationCalculator
{
    public static RecipeConfiguration CalculateRecipeConfiguration(this IReadOnlyCollection<IFeedForRecipeCreationAdapter> feedForRecipeCreationAdapters, string recipeName)
    {
        var normalizedQuantityByFeed = new Dictionary<IFeed, double>();
        double totalByUnit = 0;
        foreach (var adapter in feedForRecipeCreationAdapters)
        {
            var quantityInKg = adapter.FeedQuantity.Unit.GetNormalizedQuantityInKg(adapter.FeedQuantity.Quantity);
            // a recipe could be created from another recipe
            foreach (var subIngredient in adapter.GetUnderlyingFeeds())
            {
                var relativeQuantity = subIngredient.Percentage * quantityInKg;
                normalizedQuantityByFeed.Add(subIngredient.Feed, relativeQuantity); 
                totalByUnit += relativeQuantity;
            }
        }

        var ingredients = new List<IIngredientForRecipe>();
        foreach (var feed in normalizedQuantityByFeed)
        {
            ingredients.Add(new IngredientForRecipe(feed.Value / totalByUnit, feed.Key));
        }

        return new RecipeConfiguration(recipeName, ingredients, FeedUnit.Kg);
    }
    

    public static IReadOnlyList<IVerifyFeed> GetAllIndividualFeeds(IReadOnlyCollection<IFeedThatCouldBeAddedIntoRecipe> selectedFeeds)
    {
        var individualFeeds = new List<IVerifyFeed>(selectedFeeds.Count);
        foreach (var feedOrRecipeAdapter in selectedFeeds)
        {
            switch (feedOrRecipeAdapter)
            {
                case IRecipeAdapter recipeAdapter:
                    individualFeeds.AddRange(recipeAdapter.Ingredients);
                    break;
                case IFeedVerifySpecificAdapter feedVerifySpecificAdapter:
                    individualFeeds.Add(feedVerifySpecificAdapter);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(feedOrRecipeAdapter));
            }
        }
        return individualFeeds;
    }
    
}