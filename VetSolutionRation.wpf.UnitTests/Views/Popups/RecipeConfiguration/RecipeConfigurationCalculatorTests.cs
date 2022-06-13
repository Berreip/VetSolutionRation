using Moq;
using NUnit.Framework;
using VetSolutionRation.wpf.Views.Popups.RecipeConfiguration;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Adapters;
using VetSolutionRationLib.Enums;

namespace VetSolutionRation.wpf.UnitTests.Views.Popups.RecipeConfiguration;

[TestFixture]
internal sealed class RecipeConfigurationCalculatorTests
{
    [Test]
    public void CalculateRecipeConfiguration_same_repartition()
    {
        //Arrange
        var feed1 = CreateFeedMock(10, FeedUnit.Kg);
        var feed2 = CreateFeedMock(10, FeedUnit.Kg);
        var feed3 = CreateFeedMock(10, FeedUnit.Kg);

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
        var feed1 = CreateFeedMock(10, FeedUnit.Kg);
        var feed2 = CreateFeedMock(45, FeedUnit.Kg);
        var feed3 = CreateFeedMock(9, FeedUnit.Kg);

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
    public void CalculateRecipeConfiguration_with_another_recipe()
    {
        //Arrange
        var feedAdapter = new Mock<IFeedAdapter>();
        var feedAdapter2 = new Mock<IFeedAdapter>();

        var feed1 = CreateFeedMock(10, FeedUnit.Kg);
        var feed2 = CreateFeedMock(45, FeedUnit.Kg);

        var recipe = CreateRecipeMock(70, FeedUnit.Kg, new[]
        {
            (feedAdapter.Object, 0.3),
            (feedAdapter2.Object, 0.7),
        });

        //Act
        var res = new[] { recipe.Object, feed2.Object, feed1.Object }.CalculateRecipeConfiguration("feedName");

        //Assert
        Assert.IsNotNull(res);
        Assert.AreEqual("feedName", res.RecipeName);
        var ingredients = res.GetIngredients();
        Assert.AreEqual(4, ingredients.Count);
        Assert.AreEqual(0.1680, ingredients[0].Percentage, 0.0001);
        Assert.AreEqual(0.3920, ingredients[1].Percentage, 0.0001);
        Assert.AreEqual(0.36, ingredients[2].Percentage, 0.0001);
        Assert.AreEqual(0.08, ingredients[3].Percentage, 0.0001);
        Assert.AreEqual(1, ingredients.Sum(o => o.Percentage), 0.01);
        
    }

    [Test]
    public void CalculateRecipeConfiguration_with_another_recipe_And_duplicates()
    {
        //Arrange
        var feedAdapter = new Mock<IFeedAdapter>();
        var feedAdapter2 = new Mock<IFeedAdapter>();
        var feed2 = CreateFeedMock(45, FeedUnit.Kg);
        var recipe = CreateRecipeMock(70, FeedUnit.Kg, new[]
        {
            (feedAdapter.Object, 0.3),
            (feedAdapter2.Object, 0.7),
        });

        //Act
        var res = new[] { recipe.Object, feed2.Object }.CalculateRecipeConfiguration("feedName");

        //Assert
        Assert.IsNotNull(res);
        var ingredients = res.GetIngredients();
        Assert.AreEqual(3, ingredients.Count); // should also have feed 3
        Assert.AreEqual(0.1826, ingredients[0].Percentage, 0.0001);
        Assert.AreEqual(0.4260, ingredients[1].Percentage, 0.0001);
        Assert.AreEqual(0.3913, ingredients[2].Percentage, 0.0001);
        Assert.AreEqual(1, ingredients.Sum(o => o.Percentage), 0.01);
    }

    private static Mock<IFeedForRecipeCreationAdapter> CreateRecipeMock(int quantity, FeedUnit feedUnit, params (IFeedAdapter Feed, double Percentage)[] feeds)
    {
        var recipe = new Mock<IFeedForRecipeCreationAdapter>();
        recipe.Setup(o => o.FeedQuantity.Quantity).Returns(quantity);
        recipe.Setup(o => o.FeedQuantity.Unit).Returns(feedUnit);
        recipe.Setup(o => o.GetUnderlyingFeeds()).Returns(feeds);
        return recipe;
    }

    private static Mock<IFeedForRecipeCreationAdapter> CreateFeedMock(int quantity, FeedUnit feedUnit, IFeedAdapter feedAdapterProvided = null)
    {
        var feed = new Mock<IFeedForRecipeCreationAdapter>();
        feed.Setup(o => o.FeedQuantity.Quantity).Returns(quantity);
        feed.Setup(o => o.FeedQuantity.Unit).Returns(feedUnit);
        // single ingredient
        feed.Setup(o => o.GetUnderlyingFeeds()).Returns(new[] { (feedAdapterProvided ?? new Mock<IFeedAdapter>().Object, 1d) });
        return feed;
    }
}