using Moq;
using NUnit.Framework;
using VetSolutionRation.wpf.UnitTests.UnitTestUtils;
using VetSolutionRation.wpf.UnitTests.Views.Popups.RecipeConfiguration;
using VSR.Core.Services;
using VSR.Models.Ingredients;
using VSR.Models.Recipe;
using VSR.WPF.Utils.Adapters.IngredientsAndRecipeList;
using VSR.WPF.Utils.Services;

namespace VetSolutionRation.wpf.UnitTests.Services;

[TestFixture]
internal sealed class IngredientAdaptersHosterTests
{
    private IngredientAdaptersHoster _sut;
    private Mock<IIngredientsManager> _ingredientsManager;
    private Mock<IIngredient> _ingredient1;
    private Mock<IIngredient> _ingredient2;
    private Mock<IRecipe> _recipe1;

    [SetUp]
    public void TestInitialize()
    {
        // mock:
        _ingredientsManager = new Mock<IIngredientsManager>();
        _ingredient1 = UtilsUnitTests.CreateIngredient("init1");
        _ingredient2 = UtilsUnitTests.CreateIngredient("init2");
        _ingredientsManager.Setup(o => o.GetAllIngredients()).Returns(new List<IIngredient> { _ingredient1.Object, _ingredient2.Object });

        _recipe1 = UtilsUnitTests.CreateRecipe("recipe1");
        _ingredientsManager.Setup(o => o.GetAllRecipes()).Returns(new List<IRecipe> { _recipe1.Object });

        // software under test:
        _sut = new IngredientAdaptersHoster(_ingredientsManager.Object);
    }

    [Test]
    public void ctor_initial_setup()
    {
        //Arrange

        //Act

        //Assert
        var all = _sut.AvailableIngredientAndRecipes.ToArray<IIngredientOrRecipeForListAdapter>();
        Assert.AreEqual(3, all.Count);
        Assert.IsTrue(all.Any(o => o.Name == _ingredient1.Object.Label));
        Assert.IsTrue(all.Any(o => o.Name == _ingredient2.Object.Label));
        Assert.IsTrue(all.Any(o => o.Name == _recipe1.Object.RecipeName));
        Assert.IsNull(_sut.SearchFilter);
    }

    [Test]
    public void OnIngredientsChanged_Ingredient_Added()
    {
        //Arrange
        var ingredient3 = UtilsUnitTests.CreateIngredient("foo1");
        var monitor = new IngredientsChangeMonitor();
        monitor.SignalAdded(ingredient3.Object);

        //Act
        _ingredientsManager.Raise(m => m.OnIngredientsChanged += null, monitor);

        //Assert
        var all = _sut.AvailableIngredientAndRecipes.ToArray<IIngredientOrRecipeForListAdapter>();
        Assert.AreEqual(4, all.Count);
        Assert.IsTrue(all.Any(o => o.Name == ingredient3.Object.Label));
    }

    [Test]
    public void OnRecipesChanged_Recipe_Added()
    {
        //Arrange
        var recipe = UtilsUnitTests.CreateRecipe("foo1");
        var monitor = new RecipesChangeMonitor();
        monitor.SignalAdded(recipe.Object);

        //Act
        _ingredientsManager.Raise(m => m.OnRecipesChanged += null, monitor);

        //Assert
        var all = _sut.AvailableIngredientAndRecipes.ToArray<IIngredientOrRecipeForListAdapter>();
        Assert.AreEqual(4, all.Count);
        Assert.IsTrue(all.Any(o => o.Name == recipe.Object.RecipeName));
    }

    [Test]
    public void OnIngredientsChanged_Ingredient_Removed()
    {
        //Arrange
        var monitor = new IngredientsChangeMonitor();
        monitor.SignalRemoved(_ingredient1.Object);

        //Act
        _ingredientsManager.Raise(m => m.OnIngredientsChanged += null, monitor);

        //Assert
        var all = _sut.AvailableIngredientAndRecipes.ToArray<IIngredientOrRecipeForListAdapter>();
        Assert.AreEqual(2, all.Count);
        Assert.IsFalse(all.Any(o => o.Name == _ingredient1.Object.Label));
    }

    [Test]
    public void OnRecipesChanged_Recipe_removed()
    {
        //Arrange
        var monitor = new RecipesChangeMonitor();
        monitor.SignalRemoved(_recipe1.Object);

        //Act
        _ingredientsManager.Raise(m => m.OnRecipesChanged += null, monitor);

        //Assert
        var all = _sut.AvailableIngredientAndRecipes.ToArray<IIngredientOrRecipeForListAdapter>();
        Assert.AreEqual(2, all.Count);
        Assert.IsFalse(all.Any(o => o.Name == _recipe1.Object.RecipeName));
    }

    [Test]
    public void OnIngredientsChanged_Ingredient_Removed_then_added_again_on_same_message()
    {
        //Arrange
        var monitor = new IngredientsChangeMonitor();
        monitor.SignalRemoved(_ingredient1.Object);
        monitor.SignalAdded(_ingredient1.Object);

        //Act
        _ingredientsManager.Raise(m => m.OnIngredientsChanged += null, monitor);

        //Assert
        var all = _sut.AvailableIngredientAndRecipes.ToArray<IIngredientOrRecipeForListAdapter>();
        Assert.AreEqual(3, all.Count);
        Assert.IsTrue(all.Any(o => o.Name == _ingredient1.Object.Label));
    }

    [Test]
    public void OnRecipesChanged_Recipe_Removed_then_added_again_on_same_message()
    {
        //Arrange
        var monitor = new RecipesChangeMonitor();
        monitor.SignalRemoved(_recipe1.Object);
        monitor.SignalAdded(_recipe1.Object);

        //Act
        _ingredientsManager.Raise(m => m.OnRecipesChanged += null, monitor);

        //Assert
        var all = _sut.AvailableIngredientAndRecipes.ToArray<IIngredientOrRecipeForListAdapter>();
        Assert.AreEqual(3, all.Count);
        Assert.IsTrue(all.Any(o => o.Name == _recipe1.Object.RecipeName));
    }
}