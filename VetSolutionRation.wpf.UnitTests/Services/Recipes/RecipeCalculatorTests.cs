using Moq;
using NUnit.Framework;
using VetSolutionRation.wpf.Views.Popups.RecipeConfiguration;
using VetSolutionRation.wpf.Views.RatioPanel.Recipe;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models.Recipe;

namespace VetSolutionRation.wpf.UnitTests.Services.Recipes;

[TestFixture]
internal sealed class RecipeCalculatorTests
{
    private IRecipeCalculator _sut;

    [SetUp]
    public void TestInitialize()
    {
        // mock:

        // software under test:
        _sut = new RecipeCalculator();
    }

    private static IFeedThatCouldBeAddedIntoRecipe CreateFeedMock(bool isSelected = true)
    {
        var mock = new Mock<IFeedThatCouldBeAddedIntoRecipe>();
        mock.Setup(o => o.IsSelected).Returns(isSelected);
        return mock.Object;
    }

    [Test]
    public void CouldCalculateRecipe_returns_false_for_empty_feeds_list()
    {
        //Arrange

        //Act
        var res = _sut.CouldCalculateRecipe(Array.Empty<IFeedThatCouldBeAddedIntoRecipe>());

        //Assert
        Assert.IsFalse(res);
    }
    
    [Test]
    public void CouldCalculateRecipe_returns_false_when_only_one_ingredient()
    {
        //Arrange
        var feeds = new List<IFeedThatCouldBeAddedIntoRecipe> { CreateFeedMock() };

        //Act
        var res = _sut.CouldCalculateRecipe(feeds);

        //Assert
        Assert.IsFalse(res);
    }
    
    [Test]
    public void CouldCalculateRecipe_returns_true_when_more_than_one_ingredient()
    {
        //Arrange
        var feeds = new List<IFeedThatCouldBeAddedIntoRecipe> { CreateFeedMock(), CreateFeedMock() };

        //Act
        var res = _sut.CouldCalculateRecipe(feeds);

        //Assert
        Assert.IsTrue(res);
    }
    
    [Test]
    public void CouldCalculateRecipe_returns_false_when_more_than_one_ingredient_but_only_one_selected()
    {
        //Arrange
        var feeds = new List<IFeedThatCouldBeAddedIntoRecipe>
        {
            CreateFeedMock(false), // not selected
            CreateFeedMock(),
        };

        //Act
        var res = _sut.CouldCalculateRecipe(feeds);

        //Assert
        Assert.IsFalse(res);
    }

    [Test]
    public void CreateNewReciepe_returns_recipe_match()
    {
        //Arrange
        var recipeConfiguration = new Mock<IRecipeConfiguration>();
        recipeConfiguration.Setup(o => o.RecipeName).Returns("Name");
        recipeConfiguration.Setup(o => o.RecipeUnit).Returns(FeedUnit.Kg);

        recipeConfiguration.Setup(o => o.GetIngredients()).Returns(new []
        {
            CreateIngredientMock(10).Object,
            CreateIngredientMock(85).Object,
            CreateIngredientMock(5).Object,
        });
        
        //Act
        var res = _sut.CreateNewReciepe(recipeConfiguration.Object);

        //Assert
        Assert.IsNotNull(res);
        Assert.AreEqual("Name", res.RecipeName);
        Assert.AreEqual(FeedUnit.Kg, res.Unit);
        Assert.AreEqual(3, res.Ingredients.Count);
    }
    
    private static Mock<IIngredientForRecipe> CreateIngredientMock(int percentage)
    {
        var ingredient = new Mock<IIngredientForRecipe>();
        ingredient.Setup(o => o.Percentage).Returns(percentage);
        return ingredient;
    }
}