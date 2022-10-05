using Moq;
using NUnit.Framework;
using VetSolutionRation.wpf.UnitTests.UnitTestUtils;
using VetSolutionRation.wpf.Views;
using VetSolutionRation.wpf.Views.Calculation;
using VSR.Core.Services;
using VSR.Models.Ingredients;
using VSR.WPF.Utils.Adapters.CalculationAdapters;
using VSR.WPF.Utils.PopupManager;

namespace VetSolutionRation.wpf.UnitTests.Views;

[TestFixture]
internal sealed class VerifyRatiosViewModelTests
{
    private CalculationViewModel _sut;
    private Mock<IRecipeCalculator> _recipeCalculator;
    private Mock<IIngredientsManager> _ingredientManager;
    private Mock<IPopupManagerLight> _popupManagerLight;

    [SetUp]
    public void TestInitialize()
    {
        // mock:
        _recipeCalculator = new Mock<IRecipeCalculator>();
        _ingredientManager = new Mock<IIngredientsManager>();
        _popupManagerLight = new Mock<IPopupManagerLight>();

        // software under test:
        _sut = new CalculationViewModel(_recipeCalculator.Object, _ingredientManager.Object, _popupManagerLight.Object);
        
        // bind can execute:
        _sut.CreateRecipeCommand.BindCanExecuteChangedForUnitTests();
        _sut.RemoveFromSelectedIngredientCommand.BindCanExecuteChangedForUnitTests();
    }

    [Test]
    public void ctor_assign_correct_initial_value()
    {
        //Arrange

        //Act

        //Assert
        Assert.IsFalse(_sut.CreateRecipeCommand.CanExecute());
        Assert.IsEmpty(_sut.IngredientAndRecipeInCalculationPanel.ToArray<IIngredient>());
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void CreateRecipeCommand_can_execute_evaluated_by_calculator(bool recipeCalculatorResult)
    {
        //Arrange
        _recipeCalculator
            .Setup(o => o.CouldCalculateRecipe(It.IsAny<IReadOnlyCollection<IAdapterInCalculation>>()))
            .Returns(recipeCalculatorResult);

        //Act
        var res = _sut.CreateRecipeCommand.CanExecute();

        //Assert
        Assert.AreEqual(recipeCalculatorResult, res);
    } 
    
    [Test]
    public void CreateRecipeCommand_can_execute_evaluated_if_feed_is_added()
    {
        //Arrange
        var feed = new Mock<IIngredient>();

        //Act
        _sut.AddSelectedIngredient(feed.Object);

        //Assert
        _recipeCalculator.Verify(o => o.CouldCalculateRecipe(It.IsAny<IReadOnlyCollection<IAdapterInCalculation>>()), Times.Once);
    }
    
    [Test]
    public void CreateRecipeCommand_can_execute_evaluated_if_feed_selection_changed()
    {
        //Arrange
        var feed = new Mock<IIngredient>();
        _sut.AddSelectedIngredient(feed.Object);
        var feedAdapter = _sut.IngredientAndRecipeInCalculationPanel.ToArray<IngredientInCalculationAdapter>().Single();
        
        // reset the mock to count calls from here
        _recipeCalculator.Reset();
        
        //Act
        feedAdapter.IsSelected = false;
        
        //Assert
        _recipeCalculator.Verify(o => o.CouldCalculateRecipe(It.IsAny<IReadOnlyCollection<IAdapterInCalculation>>()), Times.Once);
    }
    
    [Test]
    public void CreateRecipeCommand_can_execute_evaluated_if_feed_is_removed()
    {
        //Arrange
        var feed = new Mock<IIngredient>();
        _sut.AddSelectedIngredient(feed.Object);
        var feedAdapter = _sut.IngredientAndRecipeInCalculationPanel.ToArray<IAdapterInCalculation>().Single();
        
        // reset the mock to count calls from here
        _recipeCalculator.Reset();

        //Act
        _sut.RemoveFromSelectedIngredientCommand.Execute(feedAdapter);

        //Assert
        _recipeCalculator.Verify(o => o.CouldCalculateRecipe(It.IsAny<IReadOnlyCollection<IAdapterInCalculation>>()), Times.Once);
    }

    [Test]
    public void AddSelectedFeed_add_once_when_not_known_before()
    {
        //Arrange
        var feed = new Mock<IIngredient>();
        feed.Setup(o => o.Guid).Returns(Guid.NewGuid());

        //Act
        _sut.AddSelectedIngredient(feed.Object);

        //Assert
        var ingredients = _sut.IngredientAndRecipeInCalculationPanel.ToArray<IngredientInCalculationAdapter>();
        Assert.AreEqual(1, ingredients.Count);
        Assert.AreSame(feed.Object, ingredients[0].GetUnderlyingIngredient());
    }
    
    [Test]
    public void AddSelectedFeed_add_once_when_duplicated()
    {
        //Arrange
        var feed = new Mock<IIngredient>();

        //Act
        for (var i = 0; i < 10; i++)
        {
            _sut.AddSelectedIngredient(feed.Object);
        }

        //Assert
        var ingredients = _sut.IngredientAndRecipeInCalculationPanel.ToArray<IngredientInCalculationAdapter>();
        Assert.AreEqual(1, ingredients.Count);
        Assert.AreSame(feed.Object, ingredients[0].GetUnderlyingIngredient());
    }
    
    [Test]
    public void AddSelectedFeed_add_recipe()
    {
        //Arrange
        var recipe = new Mock<IIngredient>();

        //Act
        _sut.AddSelectedIngredient(recipe.Object);

        //Assert
        var ingredients = _sut.IngredientAndRecipeInCalculationPanel.ToArray<IngredientInCalculationAdapter>();
        Assert.AreEqual(1, ingredients.Count);
        Assert.AreSame(recipe.Object, ingredients[0].GetUnderlyingIngredient());
    }
}