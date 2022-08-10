using System;
using System.Collections.Generic;
using System.Linq;
using VetSolutionRation.wpf.Helpers;
using VetSolutionRation.wpf.Views.Adapter;
using VetSolutionRation.wpf.Views.Popups.RecipeConfiguration.Adapters;
using VetSolutionRation.wpf.Views.RatioPanel.Recipe;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models.Recipe;

namespace VetSolutionRation.wpf.Views.Popups.RecipeConfiguration;

internal static class RecipeConfigurationCalculator
{
    public static RecipeConfiguration CalculateRecipeConfiguration(this IReadOnlyCollection<IFeedForRecipeCreationAdapter> feedForRecipeCreationAdapters, string recipeName)
    {
        var percentageByElement = NormalizedQuantityByFeedAndGetPercentage(feedForRecipeCreationAdapters);

        var ingredients = new List<IIngredientForRecipe>();
        foreach (var data in percentageByElement)
        {
            ingredients.Add(new IngredientForRecipe(data.Value, data.Key.GetUnderlyingFeed()));
        }

        return new RecipeConfiguration(recipeName, ingredients, FeedUnit.Kg);
    }

    public static Dictionary<IFeedForRecipeCreationAdapter, double> NormalizedQuantityByFeedAndGetPercentage(this IReadOnlyCollection<IFeedForRecipeCreationAdapter> feedForRecipeCreationAdapters)
    {
        var normalizedQuantityByFeed = new Dictionary<IFeedForRecipeCreationAdapter, double>();
        var totalByUnit = 0d;
        foreach (var adapter in feedForRecipeCreationAdapters)
        {
            var quantityInKg = adapter.FeedQuantity.Unit.GetNormalizedQuantityInKg(adapter.FeedQuantity.Quantity);
            normalizedQuantityByFeed.Add(adapter, quantityInKg);
            totalByUnit += quantityInKg;
        }

        var percentageByElement = new Dictionary<IFeedForRecipeCreationAdapter, double>();
        foreach (var feed in normalizedQuantityByFeed)
        {
            percentageByElement.Add(feed.Key, feed.Value / totalByUnit);
        }
        return percentageByElement;
    }


    public static IReadOnlyList<IVerifyFeed> GetAllIndividualFeeds(IReadOnlyCollection<IFeedThatCouldBeAddedIntoRecipe> selectedFeeds)
    {
        var individualFeeds = new Dictionary<Guid, IVerifyFeed>();
        foreach (var feedOrRecipeAdapter in selectedFeeds)
        {
            switch (feedOrRecipeAdapter)
            {
                case IRecipeAdapter recipeAdapter:
                    foreach (var ingredient in recipeAdapter.Ingredients)
                    {
                        AddIngredientIfNotAlreadyThere(individualFeeds, ingredient);
                    }
                    break;
                case IFeedVerifySpecificAdapter ingredient:
                    AddIngredientIfNotAlreadyThere(individualFeeds, ingredient);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(feedOrRecipeAdapter));
            }
        }
        return individualFeeds.Values.ToArray();
    }

    private static void AddIngredientIfNotAlreadyThere(Dictionary<Guid, IVerifyFeed> individualFeeds, IVerifyFeed ingredient)
    {
        if (!individualFeeds.ContainsKey(ingredient.Guid))
        {
            individualFeeds.Add(ingredient.Guid, ingredient);
        }
    }
}