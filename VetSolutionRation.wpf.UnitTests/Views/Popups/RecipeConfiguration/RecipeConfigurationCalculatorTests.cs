using Moq;
using NUnit.Framework;
using VetSolutionRation.wpf.Views.PopupRecipeConfiguration;
using VSR.Enums;
using VSR.Models.Ingredients;
using VSR.Models.Recipe;
using VSR.WPF.Utils.Adapters.CalculationAdapters;
using VSR.WPF.Utils.Adapters.EditionRecipes;

namespace VetSolutionRation.wpf.UnitTests.Views.Popups.RecipeConfiguration;

[TestFixture]
internal sealed class RecipeConfigurationCalculatorTests
{
    [Test]
    public void CalculateRecipeConfiguration_same_repartition()
    {
        //Arrange
        var feed1 = CreateFeedAdapterMock(10, FeedUnit.Kg);
        var feed2 = CreateFeedAdapterMock(10, FeedUnit.Kg);
        var feed3 = CreateFeedAdapterMock(10, FeedUnit.Kg);

        //Act
        var res = new[] { feed1.Object, feed2.Object, feed3.Object }.CalculateRecipeConfiguration("feedName");

        //Assert
        Assert.IsNotNull(res);
        var ingredients = res.GetIngredients();
        Assert.AreEqual("feedName", res.RecipeName);
        Assert.AreEqual(3, ingredients.Count);
        Assert.AreEqual(0.3333, ingredients[0].Percentage, 0.0001);
        Assert.AreEqual(0.3333, ingredients[1].Percentage, 0.0001);
        Assert.AreEqual(0.3333, ingredients[2].Percentage, 0.0001);
        Assert.AreEqual(1, ingredients.Sum(o => o.Percentage), 0.01);
    }

    [Test]
    public void CalculateRecipeConfiguration_nominal()
    {
        //Arrange
        var feed1 = CreateFeedAdapterMock(10, FeedUnit.Kg);
        var feed2 = CreateFeedAdapterMock(45, FeedUnit.Kg);
        var feed3 = CreateFeedAdapterMock(9, FeedUnit.Kg);

        //Act
        var res = new[] { feed1.Object, feed2.Object, feed3.Object }.CalculateRecipeConfiguration("feedName");

        //Assert
        Assert.IsNotNull(res);
        Assert.AreEqual("feedName", res.RecipeName);
        var ingredients = res.GetIngredients();
        Assert.AreEqual(3, ingredients.Count);
        Assert.AreEqual(0.15625d, ingredients[0].Percentage, 0.0001);
        Assert.AreEqual(0.703125d, ingredients[1].Percentage, 0.0001);
        Assert.AreEqual(0.140625, ingredients[2].Percentage, 0.0001);
        Assert.AreEqual(1, ingredients.Sum(o => o.Percentage), 0.01);
    }

    [Test]
    public void GetAllIndividualFeeds_returns_empty_empty_input()
    {
        //Arrange

        //Act
        var res = RecipeConfigurationCalculator.GetAllIndividualIngredients(Array.Empty<IAdapterInCalculation>());

        //Assert
        Assert.IsEmpty(res);
    }

    [Test]
    public void GetAllIndividualFeeds_returns_all_individual_feed_if_no_recipe()
    {
        //Arrange
        var f = UtilsUnitTests.CreateIngredientCalculationAdapter("feed1");
        var f2 = UtilsUnitTests.CreateIngredientCalculationAdapter("feed2");
        var f3 = UtilsUnitTests.CreateIngredientCalculationAdapter("feed3");

        //Act
        var res = RecipeConfigurationCalculator.GetAllIndividualIngredients(
            new List<IAdapterInCalculation>
            {
                f,
                f2,
                f3,
            });

        //Assert
        Assert.AreEqual(3, res.Count);
        Assert.AreSame(f.GetUnderlyingIngredient(), res[0].Ingredient);
        Assert.AreSame(f2.GetUnderlyingIngredient(), res[1].Ingredient);
        Assert.AreSame(f3.GetUnderlyingIngredient(), res[2].Ingredient);
    }

    [Test]
    public void GetAllIndividualFeeds_returns_recipe_ingredient_if_recipe()
    {
        //Arrange
        var f = UtilsUnitTests.CreateIngredientForRecipe("feed1");
        var f2 = UtilsUnitTests.CreateIngredientForRecipe("feed2");
        var f3 = UtilsUnitTests.CreateIngredientForRecipe("feed3");
        var recipe = new Mock<IRecipe>();
        recipe.Setup(o => o.IngredientsForRecipe)
            .Returns(new[]
            {
                f,
                f2,
                f3,
            });

        //Act
        var res = RecipeConfigurationCalculator.GetAllIndividualIngredients(new List<IAdapterInCalculation>
        {
            new RecipeInCalculationAdapter(recipe.Object),
        });

        //Assert
        Assert.AreEqual(3, res.Count);
        Assert.AreSame(f.Ingredient, res[0].Ingredient);
        Assert.AreSame(f2.Ingredient, res[1].Ingredient);
        Assert.AreSame(f3.Ingredient, res[2].Ingredient);
    }

    [Test]
    public void GetAllIndividualFeeds_returns_recipe_ingredients_and_individuals_if_recipe_mixed_with_single_ingredient()
    {
        //Arrange
        var f = UtilsUnitTests.CreateIngredientForRecipe("feed1");
        var f2 = UtilsUnitTests.CreateIngredientForRecipe("feed2");
        var recipe = new Mock<IRecipe>();
        recipe.Setup(o => o.IngredientsForRecipe).Returns(new[] { f, f2 });
        
        var f3 =  UtilsUnitTests.CreateIngredientCalculationAdapter("feed3");

        //Act
        var res = RecipeConfigurationCalculator.GetAllIndividualIngredients(new List<IAdapterInCalculation>
        {
            new RecipeInCalculationAdapter(recipe.Object), // recipe with 2 ingredients
            f3, // single ingredient
        });

        //Assert
        Assert.AreEqual(3, res.Count);
        Assert.AreSame(f.Ingredient, res[0].Ingredient);
        Assert.AreSame(f2.Ingredient, res[1].Ingredient);
        Assert.AreSame(f3.GetUnderlyingIngredient(), res[2].Ingredient);
    }

    [Test]
    public void GetAllIndividualFeeds_filter_duplicates()
    {
        //Arrange
        var feed1 = UtilsUnitTests.CreateIngredientCalculationAdapter("feed1");

        //Act
        var res = RecipeConfigurationCalculator.GetAllIndividualIngredients(new List<IAdapterInCalculation>
        {
            feed1,
            feed1,
        });

        //Assert
        Assert.AreEqual(1, res.Count);
        Assert.AreEqual(feed1.Guid, res[0].Ingredient.Guid);
    }

    [Test]
    public void GetAllIndividualFeeds_filter_duplicates_if_other_not_duplicated()
    {
        //Arrange
        var feed1 = UtilsUnitTests.CreateIngredientCalculationAdapter("feed1");
        var feed2 = UtilsUnitTests.CreateIngredientCalculationAdapter("feed1");

        //Act
        var res = RecipeConfigurationCalculator.GetAllIndividualIngredients(new List<IAdapterInCalculation>
        {
            feed1,
            feed1,
            feed2,
        });

        //Assert
        Assert.AreEqual(2, res.Count);
        Assert.IsTrue(res.Any(o => o.Ingredient == feed1.GetUnderlyingIngredient()));
        Assert.IsTrue(res.Any(o => o.Ingredient == feed2.GetUnderlyingIngredient()));
    }

    [Test]
    public void GetAllIndividualFeeds_returns_recipe_ingredients_and_individuals_even_if_provided_multiple_times()
    {
        //Arrange
        var feed1 = UtilsUnitTests.CreateIngredientCalculationAdapter("feed1");
        var feed2 = UtilsUnitTests.CreateIngredientCalculationAdapter("feed2");
        var feed3 = UtilsUnitTests.CreateIngredientCalculationAdapter("feed3");
        var recipe = new Mock<IRecipe>();
        // the recipe already contains all previous feed except feed3
        var ingredient1 = new IngredientForRecipe(10, feed2.GetUnderlyingIngredient());
        var ingredient2 = new IngredientForRecipe(10, feed3.GetUnderlyingIngredient());
        recipe.Setup(o => o.IngredientsForRecipe).Returns(new[] { ingredient1, ingredient2 });

        //Act
        var res = RecipeConfigurationCalculator.GetAllIndividualIngredients(new List<IAdapterInCalculation>
        {
            feed1,
            feed2,
            feed3,
            new RecipeInCalculationAdapter(recipe.Object),
        });

        //Assert
        Assert.AreEqual(3, res.Count);
        Assert.IsTrue(res.Any(o => o.Ingredient == feed1.GetUnderlyingIngredient()));
        Assert.IsTrue(res.Any(o => o.Ingredient == feed2.GetUnderlyingIngredient()));
        Assert.IsTrue(res.Any(o => o.Ingredient == feed3.GetUnderlyingIngredient()));
    }


    private static Mock<IIngredientInRecipeCreationAdapter> CreateFeedAdapterMock(int quantity, FeedUnit feedUnit, IIngredient feedProvided = null)
    {
        var adapter = new Mock<IIngredientInRecipeCreationAdapter>();
        adapter.Setup(o => o.Quantity).Returns(quantity);
        adapter.Setup(o => o.Unit).Returns(feedUnit);
        // single ingredient
        adapter.Setup(o => o.GetUnderlyingIngredient()).Returns(feedProvided ?? new Mock<IIngredient>().Object);
        return adapter;
    }
}