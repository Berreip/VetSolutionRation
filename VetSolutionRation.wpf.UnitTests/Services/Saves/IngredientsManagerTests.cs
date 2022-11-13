using Moq;
using NUnit.Framework;
using PRF.Utils.CoreComponents.Extensions;
using VetSolutionRation.DataProvider.Models.Helpers;
using VetSolutionRation.wpf.UnitTests.Views.Popups.RecipeConfiguration;
using VSR.Core.Services;
using VSR.Dto;
using VSR.Enums;
using VSR.Models.Ingredients;
using VSR.Models.Recipe;
using VSR.WPF.Utils.Helpers;
using VSR.WPF.Utils.Services.Configuration;

namespace VetSolutionRation.wpf.UnitTests.Services.Saves;

[TestFixture]
internal sealed class IngredientsManagerTests
{
    private IIngredientsManager _sut;

    [SetUp]
    public void TestInitialize()
    {
        // mock:

        // software under test:
        _sut = new IngredientsManager();
    }

    private static Mock<IIngredientForRecipe> CreateIngredientForRecipe(double percentage, IIngredient ingredient)
    {
        var ingredientForRecipe = new Mock<IIngredientForRecipe>();
        ingredientForRecipe.Setup(o => o.Percentage).Returns(percentage);
        ingredientForRecipe.Setup(o => o.Ingredient).Returns(ingredient);
        return ingredientForRecipe;
    }

    [Test]
    public void AddIngredient_do_not_raise_OnFeedChanged_event_when_empty()
    {
        //Arrange
        var count = 0;
        _sut.OnIngredientsChanged += _ => Interlocked.Increment(ref count);

        //Act
        _sut.AddIngredients(Array.Empty<IIngredient>());

        //Assert
        Assert.AreEqual(0, count);
    }

    [Test]
    public void AddIngredient_raise_OnFeedChanged_event_once()
    {
        //Arrange
        var monitors = new List<IIngredientsChangeMonitor>();
        _sut.OnIngredientsChanged += o => monitors.Add(o);

        //Act
        _sut.AddIngredients(new List<IIngredient> { UtilsUnitTests.CreateIngredient("foo_label").Object });

        //Assert
        Assert.AreEqual(1, monitors.Count);
    }

    [Test]
    public void AddIngredient_raise_OnFeedChanged_event_once_with_no_Removed_data()
    {
        //Arrange
        var monitors = new List<IIngredientsChangeMonitor>();
        _sut.OnIngredientsChanged += o => monitors.Add(o);
        var ingredient = UtilsUnitTests.CreateIngredient("foo_label");

        //Act
        _sut.AddIngredients(new List<IIngredient> { ingredient.Object });

        //Assert
        var monitor = monitors.Single();
        Assert.AreEqual(0, monitor.GetRemoved().Count);
    }

    [Test]
    public void AddIngredient_raise_OnFeedChanged_event_once_with_one_added_data()
    {
        //Arrange
        var monitors = new List<IIngredientsChangeMonitor>();
        _sut.OnIngredientsChanged += o => monitors.Add(o);
        var ingredient = UtilsUnitTests.CreateIngredient("foo_label");

        //Act
        _sut.AddIngredients(new List<IIngredient> { ingredient.Object });

        //Assert
        var monitor = monitors.Single();
        Assert.AreSame(ingredient.Object, monitor.GetAdded().Single());
    }

    [Test]
    public void AddRecipes_raise_OnFeedChanged_event_once_with_no_Removed_data()
    {
        //Arrange
        var monitors = new List<IRecipesChangeMonitor>();
        _sut.OnRecipesChanged += o => monitors.Add(o);

        var ingredient = UtilsUnitTests.CreateIngredient("foo_label");
        var ingredientForRecipe = CreateIngredientForRecipe(1d, ingredient.Object);
        var recipe = UtilsUnitTests.CreateRecipeCandidate(ingredientForRecipe.Object);

        //Act
        // recipe ingredient must be known before adding the recipe otherwise, it will be ignored
        _sut.AddIngredients(new List<IIngredient> { ingredient.Object });
        _sut.AddRecipes(new[] { recipe.Object });

        //Assert
        var monitor = monitors.Single();
        Assert.AreEqual(0, monitor.GetRemoved().Count);
    }

    [Test]
    public void AddRecipes_raise_OnRecipesChanged_event_once_with_one_added_data()
    {
        //Arrange
        var monitors = new List<IRecipesChangeMonitor>();
        _sut.OnRecipesChanged += o => monitors.Add(o);

        var ingredient = UtilsUnitTests.CreateIngredient("foo_label");
        var ingredientForRecipe = CreateIngredientForRecipe(1d, ingredient.Object);
        var recipe = UtilsUnitTests.CreateRecipeCandidate(ingredientForRecipe.Object);

        //Act
        // recipe ingredient must be known before adding the recipe otherwise, it will be ignored
        _sut.AddIngredients(new List<IIngredient> { ingredient.Object });
        _sut.AddRecipes(new[] { recipe.Object });

        //Assert
        var monitor = monitors.Single();
        Assert.AreEqual(recipe.Object.Guid, monitor.GetAdded().Single().Guid);
    }

    [Test]
    public void AddRecipes_do_not_add_recipe_if_no_ingredient_match()
    {
        //Arrange
        var monitors = new List<IRecipesChangeMonitor>();
        _sut.OnRecipesChanged += o => monitors.Add(o);

        var ingredientForRecipe = CreateIngredientForRecipe(1d, UtilsUnitTests.CreateIngredient("foo_label").Object);
        var recipe = UtilsUnitTests.CreateRecipeCandidate(ingredientForRecipe.Object);

        //Act
        // NO registering of recipe ingredient ==> recipe will be ignored
        _sut.AddRecipes(new[] { recipe.Object });

        //Assert
        Assert.AreEqual(0, monitors.Count);
    }

    [Test]
    public void AddRecipes_nominal_adding()
    {
        //Arrange
        var monitors = new List<IRecipesChangeMonitor>();
        _sut.OnRecipesChanged += o => monitors.Add(o);

        var ingredient = UtilsUnitTests.CreateIngredient("foo_label");
        var ingredientForRecipe = CreateIngredientForRecipe(1d, ingredient.Object);
        var recipe = UtilsUnitTests.CreateRecipeCandidate(ingredientForRecipe.Object);
        // recipe ingredient must be known before adding the recipe otherwise, it will be ignored
        _sut.AddIngredients(new List<IIngredient> { ingredient.Object });

        //Act
        _sut.AddRecipes(new[] { recipe.Object });
        

        //Assert
        Assert.AreEqual(1, monitors.Single().GetAdded().Count);
    }

    [Test]
    public void DeleteRecipe_nominal()
    {
        //Arrange
        var monitors = new List<IRecipesChangeMonitor>();

        var ingredient = UtilsUnitTests.CreateIngredient("foo_label");
        var ingredientForRecipe = CreateIngredientForRecipe(1d, ingredient.Object);
        var recipe = UtilsUnitTests.CreateRecipeCandidate(ingredientForRecipe.Object);
        
        // recipe ingredient must be known before adding the recipe otherwise, it will be ignored
        _sut.AddIngredients(new List<IIngredient> { ingredient.Object });
        var added = _sut.AddRecipes(new[] { recipe.Object });
        
        _sut.OnRecipesChanged += o => monitors.Add(o);
        
        
        //Act
        _sut.DeleteRecipe(added.Single());
        

        //Assert
        var monitor = monitors.Single();
        Assert.AreEqual(0, monitor.GetAdded().Count);
        Assert.AreEqual(1, monitor.GetRemoved().Count);
    }
}