using Moq;
using NUnit.Framework;
using VetSolutionRation.wpf.Views.Popups.RecipeConfiguration;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.Recipe;
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

    [Test]
    public void CouldCalculateRecipe_returns_false_for_empty_feeds_list()
    {
        //Arrange

        //Act
        var res = _sut.CouldCalculateRecipe(Array.Empty<IFeedThatCouldBeAddedIntoReciepe>());

        //Assert
        Assert.IsFalse(res);
    }
    
    [Test]
    public void CouldCalculateRecipe_returns_false_when_only_one_ingredient()
    {
        //Arrange
        var feeds = new List<IFeedThatCouldBeAddedIntoReciepe>
        {
            new Mock<IFeedThatCouldBeAddedIntoReciepe>().Object,
        };

        //Act
        var res = _sut.CouldCalculateRecipe(feeds);

        //Assert
        Assert.IsFalse(res);
    }
    
    [Test]
    public void CouldCalculateRecipe_returns_true_when_more_than_one_ingredient()
    {
        //Arrange
        var feeds = new List<IFeedThatCouldBeAddedIntoReciepe>
        {
            new Mock<IFeedThatCouldBeAddedIntoReciepe>().Object,
            new Mock<IFeedThatCouldBeAddedIntoReciepe>().Object,
        };

        //Act
        var res = _sut.CouldCalculateRecipe(feeds);

        //Assert
        Assert.IsTrue(res);
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
        Assert.AreEqual(0, res.Quantity);
        
    }

    private static Mock<IFeedForRecipe> CreateIngredientMock(int percentage)
    {
        var ingredient = new Mock<IFeedForRecipe>();
        ingredient.Setup(o => o.Percentage).Returns(percentage);
        return ingredient;
    }
}