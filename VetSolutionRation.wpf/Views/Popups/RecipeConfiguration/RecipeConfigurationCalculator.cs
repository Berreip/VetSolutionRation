using System.Collections.Generic;
using System.Linq;
using VetSolutionRation.wpf.Helpers;
using VetSolutionRation.wpf.Views.Adapter;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models.Recipe;

namespace VetSolutionRation.wpf.Views.Popups.RecipeConfiguration;

internal static class RecipeConfigurationCalculator
{
    public static RecipeConfiguration CalculateRecipeConfiguration(this IReadOnlyCollection<IFeedForRecipeCreationAdapter> feedForRecipeCreationAdapters, string recipeName)
    {
        var normalizedQuantityByFeed = new Dictionary<IFeedAdapter, double>();
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

        var ingredients = new List<IFeedForRecipe>();
        foreach (var feed in normalizedQuantityByFeed)
        {
            ingredients.Add(new FeedForRecipe(feed.Value / totalByUnit, feed.Key));
        }

        return new RecipeConfiguration(recipeName, ingredients, FeedUnit.Kg);
    }
    
}