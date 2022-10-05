using Moq;
using VSR.Enums;
using VSR.Models.Ingredients;
using VSR.Models.Recipe;
using VSR.WPF.Utils.Adapters.CalculationAdapters;

namespace VetSolutionRation.wpf.UnitTests.Views.Popups.RecipeConfiguration;

internal static class UtilsUnitTests
{
    public static IngredientInCalculationAdapter CreateIngredientCalculationAdapter(string name)
    {
        var ingredient = new Mock<IIngredient>();
        ingredient.Setup(o => o.Label).Returns(name);
        ingredient.Setup(o => o.Guid).Returns(Guid.NewGuid());
        var adapter = new IngredientInCalculationAdapter(ingredient.Object, o => {});
        adapter.IngredientQuantity.QuantityString = "1";
        return adapter;
    }

    public static IngredientForRecipe CreateIngredientForRecipe(string name)
    {
        var ingredient = new Mock<IIngredient>();
        ingredient.Setup(o => o.Label).Returns(name);
        ingredient.Setup(o => o.Guid).Returns(Guid.NewGuid());
        return new IngredientForRecipe(10, ingredient.Object);
    }

    public static Mock<IIngredient> CreateIngredient(string label, bool isUserAdded = false)
    {
        var feed = new Mock<IIngredient>();
        feed.Setup(o => o.Guid).Returns(Guid.NewGuid());
        feed.Setup(o => o.Label).Returns(label);
        feed.Setup(o => o.IsUserAdded).Returns(isUserAdded);
        feed.Setup(o => o.GetNutritionDetails()).Returns(new List<INutritionalDetails>());
        return feed;
    }
    
    public static Mock<IRecipe> CreateRecipe(string recipeName, params IIngredientForRecipe[] ingredients)
    {
        var recipe = new Mock<IRecipe>();
        recipe.Setup(o => o.RecipeName).Returns(recipeName);
        recipe.Setup(o => o.Guid).Returns(Guid.NewGuid());
        recipe.Setup(o => o.Unit).Returns(FeedUnit.Kg);
        recipe.Setup(o => o.IngredientsForRecipe).Returns(ingredients.ToList());
        return recipe;
    }
    
    public static Mock<IRecipeCandidate> CreateRecipeCandidate(params IIngredientForRecipe[] ingredients)
    {
        var recipe = new Mock<IRecipeCandidate>();
        recipe.Setup(o => o.Name).Returns(Guid.NewGuid().ToString()[..8]);
        recipe.Setup(o => o.Guid).Returns(Guid.NewGuid());
        var ingredientForRecipeCandidates = ingredients.Select(o => new IngredientForRecipeCandidate(1, o.Ingredient.Guid)).ToList();
        recipe.Setup(o => o.Ingredients).Returns(ingredientForRecipeCandidates);
        return recipe;
    }

}