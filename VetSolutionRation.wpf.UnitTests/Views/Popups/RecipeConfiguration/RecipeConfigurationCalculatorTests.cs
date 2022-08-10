using Moq;
using NUnit.Framework;
using VetSolutionRation.wpf.Views.Adapter;
using VetSolutionRation.wpf.Views.Popups.RecipeConfiguration;
using VetSolutionRation.wpf.Views.Popups.RecipeConfiguration.Adapters;
using VetSolutionRation.wpf.Views.RatioPanel.Recipe;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models.Feed;

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
    public void CalculateRecipeConfiguration_with_another_recipe()
    {
        //Arrange
        var f = new Mock<IFeed>();
        var f2 = new Mock<IFeed>();

        var feed1 = CreateFeedAdapterMock(10, FeedUnit.Kg);
        var feed2 = CreateFeedAdapterMock(45, FeedUnit.Kg);

        var recipe = CreateRecipeMock(70, FeedUnit.Kg, new[]
        {
            (f.Object, 0.3),
            (f2.Object, 0.7),
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
        var f = new Mock<IFeed>();
        var f2 = new Mock<IFeed>();
        var feed2 = CreateFeedAdapterMock(45, FeedUnit.Kg);
        var recipe = CreateRecipeMock(70, FeedUnit.Kg, new[]
        {
            (f.Object, 0.3),
            (f2.Object, 0.7),
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

    [Test]
    public void GetAllIndividualFeeds_returns_empty_empty_input()
    {
        //Arrange

        //Act
        var res = RecipeConfigurationCalculator.GetAllIndividualFeeds(Array.Empty<IFeedThatCouldBeAddedIntoRecipe>());

        //Assert
        Assert.IsEmpty(res);
    }

    [Test]
    public void GetAllIndividualFeeds_returns_all_individual_feed_if_no_recipe()
    {
        //Arrange
        var f = CreateFeed("feed1");
        var f2 = CreateFeed("feed2");
        var f3 = CreateFeed("feed3");

        //Act
        var res = RecipeConfigurationCalculator.GetAllIndividualFeeds(
            new List<IFeedThatCouldBeAddedIntoRecipe>
            {
                f.Object,
                f2.Object,
                f3.Object,
            });

        //Assert
        Assert.AreEqual(3, res.Count);
        Assert.AreSame(f.Object, res[0]);
        Assert.AreSame(f2.Object, res[1]);
        Assert.AreSame(f3.Object, res[2]);
    }

    [Test]
    public void GetAllIndividualFeeds_returns_recipe_ingredient_if_recipe()
    {
        //Arrange
        var f = CreateFeed("feed1");
        var f2 = CreateFeed("feed2");
        var f3 = CreateFeed("feed3");
        var recipe = new Mock<IRecipeAdapter>();
        recipe.Setup(o => o.Ingredients)
            .Returns(new List<IVerifyFeed>
            {
                f.Object,
                f2.Object,
                f3.Object,
            });

        //Act
        var res = RecipeConfigurationCalculator.GetAllIndividualFeeds(new List<IFeedThatCouldBeAddedIntoRecipe>
        {
            recipe.Object,
        });

        //Assert
        Assert.AreEqual(3, res.Count);
        Assert.AreSame(f.Object, res[0]);
        Assert.AreSame(f2.Object, res[1]);
        Assert.AreSame(f3.Object, res[2]);
    }

    [Test]
    public void GetAllIndividualFeeds_returns_recipe_ingredients_and_individuals_if_recipe_mixed_with_single_ingredient()
    {
        //Arrange
        var f = CreateFeed("feed1");
        var f2 = CreateFeed("feed2");
        var recipe = new Mock<IRecipeAdapter>();
        recipe.Setup(o => o.Ingredients)
            .Returns(new List<IVerifyFeed>
            {
                f.Object,
                f2.Object,
            });

        var f3 = new Mock<IFeedVerifySpecificAdapter>();
        
        //Act
        var res = RecipeConfigurationCalculator.GetAllIndividualFeeds(new List<IFeedThatCouldBeAddedIntoRecipe>
        {
            recipe.Object, // recipe with 2 ingredients
            f3.Object, // single ingredient
        });

        //Assert
        Assert.AreEqual(3, res.Count);
        Assert.AreSame(f.Object, res[0]);
        Assert.AreSame(f2.Object, res[1]);
        Assert.AreSame(f3.Object, res[2]);
    }
    
    [Test]
    public void GetAllIndividualFeeds_filter_duplicates()
    {
        //Arrange
        var feed1 = CreateFeed("feed1");
        
        //Act
        var res = RecipeConfigurationCalculator.GetAllIndividualFeeds(new List<IFeedThatCouldBeAddedIntoRecipe>
        {
            feed1.Object,
            feed1.Object, 
        });

        //Assert
        Assert.AreEqual(1, res.Count);
        Assert.AreSame(feed1.Object, res[0]);
    } 
    
    [Test]
    public void GetAllIndividualFeeds_filter_duplicates_if_other_not_duplicated()
    {
        //Arrange
        var feed1 = CreateFeed("feed1");
        var feed2 = CreateFeed("feed1");
        
        //Act
        var res = RecipeConfigurationCalculator.GetAllIndividualFeeds(new List<IFeedThatCouldBeAddedIntoRecipe>
        {
            feed1.Object,
            feed1.Object, 
            feed2.Object, 
        });

        //Assert
        Assert.AreEqual(2, res.Count);
        Assert.IsTrue(res.Contains(feed1.Object));
        Assert.IsTrue(res.Contains(feed2.Object));
    } 
    
    [Test]
    public void GetAllIndividualFeeds_returns_recipe_ingredients_and_individuals_even_if_provided_multiple_times()
    {
        //Arrange
        var feed1 = CreateFeed("feed1");
        var feed2 = CreateFeed("feed2");
        var feed3 = CreateFeed("feed3");
        var recipe = new Mock<IRecipeAdapter>();
        // the recipe already contains all previous feed except feed3
        recipe.Setup(o => o.Ingredients).Returns(new List<IVerifyFeed> { feed1.Object, feed2.Object });
      
        //Act
        var res = RecipeConfigurationCalculator.GetAllIndividualFeeds(new List<IFeedThatCouldBeAddedIntoRecipe>
        { 
            feed1.Object,
            feed2.Object,
            feed3.Object,
            recipe.Object,
        });

        //Assert
        Assert.AreEqual(3, res.Count);
        Assert.IsTrue(res.Contains(feed1.Object));
        Assert.IsTrue(res.Contains(feed2.Object));
        Assert.IsTrue(res.Contains(feed3.Object));
    }

    private static Mock<IFeedForRecipeCreationAdapter> CreateRecipeMock(int quantity, FeedUnit feedUnit, params (IFeed Feed, double Percentage)[] feeds)
    {
        var recipe = new Mock<IFeedForRecipeCreationAdapter>();
        recipe.Setup(o => o.FeedQuantity.Quantity).Returns(quantity);
        recipe.Setup(o => o.FeedQuantity.Unit).Returns(feedUnit);
        recipe.Setup(o => o.GetUnderlyingFeeds()).Returns(feeds);
        return recipe;
    }

    private static Mock<IFeedForRecipeCreationAdapter> CreateFeedAdapterMock(int quantity, FeedUnit feedUnit, IFeed feedProvided = null)
    {
        var adapter = new Mock<IFeedForRecipeCreationAdapter>();
        adapter.Setup(o => o.FeedQuantity.Quantity).Returns(quantity);
        adapter.Setup(o => o.FeedQuantity.Unit).Returns(feedUnit);
        // single ingredient
        adapter.Setup(o => o.GetUnderlyingFeeds()).Returns(new[] { (feedProvided ?? new Mock<IFeed>().Object, 1d) });
        return adapter;
    }
    
    private static Mock<IFeedVerifySpecificAdapter> CreateFeed(string name)
    {
        var feed = new Mock<IFeedVerifySpecificAdapter>();
        feed.Setup(o => o.Name).Returns(name);
        feed.Setup(o => o.Guid).Returns(Guid.NewGuid());
        var qty = new Mock<IFeedQuantityAdapter>();
        qty.Setup(o => o.Unit).Returns(FeedUnit.Kg);
        qty.Setup(o => o.Quantity).Returns(1);
        feed.Setup(o => o.FeedQuantity).Returns(qty.Object);
        return feed;
    }
}