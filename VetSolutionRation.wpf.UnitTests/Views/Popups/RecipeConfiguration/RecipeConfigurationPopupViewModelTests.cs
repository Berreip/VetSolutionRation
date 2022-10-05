using Moq;
using NUnit.Framework;
using VetSolutionRation.wpf.UnitTests.UnitTestUtils;
using VetSolutionRation.wpf.Views.RecipeConfiguration;
using VSR.Core.Services;
using VSR.Models.Ingredients;
using VSR.Models.Recipe;
using VSR.WPF.Utils.Adapters.CalculationAdapters;
using VSR.WPF.Utils.Adapters.EditionRecipes;
using VSR.WPF.Utils.PopupManager;

namespace VetSolutionRation.wpf.UnitTests.Views.Popups.RecipeConfiguration;

[TestFixture]
internal sealed class RecipeConfigurationPopupViewModelTests
{
    private RecipeConfigurationPopupViewModel _sut;
    private Mock<IPopupManagerLight> _popupManager;
    private Mock<IIngredientsManager> _ingredientsManager;
    private List<IAdapterInCalculation> _selectedFeed;
    private IngredientInCalculationAdapter _feed1;
    private IngredientInCalculationAdapter _feed2;

    [SetUp]
    public void TestInitialize()
    {
        // mock:
        _popupManager = new Mock<IPopupManagerLight>();
        _ingredientsManager = new Mock<IIngredientsManager>();

        _feed1 = UtilsUnitTests.CreateIngredientCalculationAdapter("feed1_name");
        _feed2 = UtilsUnitTests.CreateIngredientCalculationAdapter("feed2_name");
     
        _feed1.IngredientQuantity.QuantityString = "60";
        _feed2.IngredientQuantity.QuantityString = "20";

        _selectedFeed = new List<IAdapterInCalculation>
        {
            _feed1,
            _feed2,
        };

        // software under test:
        _sut = new RecipeConfigurationPopupViewModel(_popupManager.Object, _ingredientsManager.Object, _selectedFeed);
    }

    [Test]
    public void ctor_assign_expectedValues()
    {
        //Arrange

        //Act
        var res = _sut.SelectedIngredients.ToArray<IngredientInRecipeCreationAdapter>();

        //Assert
        Assert.AreEqual(2, res.Count);
        Assert.AreEqual("feed1_name", res[0].Name);
        Assert.AreEqual("feed2_name", res[1].Name);
        Assert.AreNotSame(60d, res[0].Quantity);
        Assert.AreNotSame(20d, res[1].Quantity);
        Assert.IsFalse(_sut.ValidateRecipeCreationCommand.CanExecute());
    }

    [Test]
    public void ChangingName_raise_can_execute_validation_command()
    {
        //Arrange

        //Act
        _sut.RecipeName = "stubName";

        //Assert
        Assert.IsTrue(_sut.ValidateRecipeCreationCommand.CanExecute());
    }

    [Test]
    public void Execute_validate_setup_recipe_configuration()
    {
        //Arrange
        _sut.RecipeName = "stubName";

        //Act
        _sut.ValidateRecipeCreationCommand.Execute();

        //Assert
        var config = _sut.RecipeConfiguration;
        Assert.IsNotNull(config);

        var ingredients = config.GetIngredients();
        Assert.AreEqual(2, ingredients.Count);
        Assert.AreEqual(0.75d, ingredients[0].Percentage);
        Assert.AreEqual(0.25d, ingredients[1].Percentage);
    }


    [Test]
    public void Ctor_do_not_duplicate_ingredient_even_if_provided_multiple_times()
    {
        //Arrange
        var feed1 = UtilsUnitTests.CreateIngredientCalculationAdapter("feed1");
        var feed2 = UtilsUnitTests.CreateIngredientCalculationAdapter("feed2");
        var feed3 = UtilsUnitTests.CreateIngredientCalculationAdapter("feed3");
        var recipe = new Mock<IRecipe>();
        // the recipe already contains all previous feed except feed3
        recipe.Setup(o => o.IngredientsForRecipe).Returns(new []
        {
            new IngredientForRecipe(50, feed1.GetUnderlyingIngredient()), 
            new IngredientForRecipe(50, feed2.GetUnderlyingIngredient()),
        });

        var feeds = new List<IAdapterInCalculation>
        {
            feed1,
            feed2,
            feed3,
            new RecipeInCalculationAdapter(recipe.Object),
        };

        //Act
        var sut = new RecipeConfigurationPopupViewModel(_popupManager.Object, _ingredientsManager.Object, feeds);

        //Assert
        var selectedFeeds = sut.SelectedIngredients.ToArray<IngredientInRecipeCreationAdapter>();
        Assert.AreEqual(3, selectedFeeds.Count);
        Assert.IsTrue(selectedFeeds.Any(o => o.Name == feed1.Name));
        Assert.IsTrue(selectedFeeds.Any(o => o.Name == feed2.Name));
        Assert.IsTrue(selectedFeeds.Any(o => o.Name == feed3.Name));
    }
}