using System;
using System.Collections.Generic;
using System.Linq;
using VSR.Enums;
using VSR.Models.Ingredients;
using VSR.WPF.Utils.Adapters.CalculationAdapters;
using VSR.WPF.Utils.Adapters.EditionRecipes;
using VSR.WPF.Utils.Helpers;

namespace VetSolutionRation.wpf.Views.RecipeConfiguration;

internal static class RecipeConfigurationCalculator
{
    public static RecipeConfiguration CalculateRecipeConfiguration(this IReadOnlyCollection<IIngredientInRecipeCreationAdapter> feedForRecipeCreationAdapters, string recipeName)
    {
        var percentageByElement = NormalizedQuantityByIngredientAndGetPercentage(feedForRecipeCreationAdapters);

        var ingredients = new List<IIngredientForRecipe>();
        foreach (var data in percentageByElement)
        {
            ingredients.Add(new IngredientForRecipe(data.Value, data.Key.GetUnderlyingIngredient()));
        }

        return new RecipeConfiguration(recipeName, ingredients, FeedUnit.Kg);
    }

    /// <summary>
    /// from a quantity for each ingredient, calculate the corresponding percentage 
    /// </summary>
    public static Dictionary<IIngredientInRecipeCreationAdapter, double> NormalizedQuantityByIngredientAndGetPercentage(this IEnumerable<IIngredientInRecipeCreationAdapter> feedForRecipeCreationAdapters)
    {
        var normalizedQuantityByFeed = new Dictionary<IIngredientInRecipeCreationAdapter, double>();
        var totalByUnit = 0d;
        foreach (var adapter in feedForRecipeCreationAdapters)
        {
            var quantityInKg = adapter.Unit.GetNormalizedQuantityInKg(adapter.Quantity);
            normalizedQuantityByFeed.Add(adapter, quantityInKg);
            totalByUnit += quantityInKg;
        }

        var percentageByElement = new Dictionary<IIngredientInRecipeCreationAdapter, double>();
        foreach (var feed in normalizedQuantityByFeed)
        {
            percentageByElement.Add(feed.Key, feed.Value / totalByUnit);
        }

        return percentageByElement;
    }


    public static IReadOnlyList<ISingleIngredientAndQuantity> GetAllIndividualIngredients(IEnumerable<IAdapterInCalculation> selectedAdapters)
    {
        var individualIngredientsById = new Dictionary<Guid, SingleIngredientAndQuantity>();
        foreach (var feedOrRecipeAdapter in selectedAdapters)
        {
            switch (feedOrRecipeAdapter)
            {
                case RecipeInCalculationAdapter recipeAdapter:
                    foreach (var ingredient in recipeAdapter.Ingredients)
                    {
                        AddIngredient(individualIngredientsById, ingredient.GetUnderlyingIngredient(), ingredient.Quantity);
                    }

                    break;
                case IngredientInCalculationAdapter ingredient:
                    AddIngredient(individualIngredientsById, ingredient.GetUnderlyingIngredient(), ingredient.IngredientQuantity.Quantity);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(feedOrRecipeAdapter));
            }
        }

        return individualIngredientsById.Values.ToArray();
    }

    private static void AddIngredient(Dictionary<Guid, SingleIngredientAndQuantity> individualIngredientsById, IIngredient ingredient, int quantity)
    {
        if (individualIngredientsById.TryGetValue(ingredient.Guid, out var data))
        {
            data.IncrementQuantity(quantity);
        }
        else
        {
            individualIngredientsById.Add(ingredient.Guid, new SingleIngredientAndQuantity(ingredient, quantity));
        }
    }

    private sealed class SingleIngredientAndQuantity : ISingleIngredientAndQuantity
    {

        /// <inheritdoc />
        public IIngredient Ingredient { get; }

        /// <inheritdoc />
        public int Quantity { get; private set; }

        public SingleIngredientAndQuantity(IIngredient ingredient, int quantity)
        {
            Ingredient = ingredient;
            Quantity = quantity;
        }
        
        public void IncrementQuantity(int quantity)
        {
            Quantity += quantity;
        }
    }
}

internal interface ISingleIngredientAndQuantity
{
    IIngredient Ingredient { get; }
    
    /// <summary>
    /// The absolute quantity
    /// </summary>
    int Quantity { get; }
}