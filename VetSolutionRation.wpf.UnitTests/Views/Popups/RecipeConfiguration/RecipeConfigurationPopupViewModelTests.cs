using Moq;
using NUnit.Framework;
using VetSolutionRation.wpf.Services.PopupManager;
using VetSolutionRation.wpf.Services.Saves;
using VetSolutionRation.wpf.UnitTests.UnitTestUtils;
using VetSolutionRation.wpf.Views.Adapter;
using VetSolutionRation.wpf.Views.Popups.RecipeConfiguration;
using VetSolutionRation.wpf.Views.Popups.RecipeConfiguration.Adapters;
using VetSolutionRation.wpf.Views.RatioPanel.Recipe;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models.Feed;

namespace VetSolutionRation.wpf.UnitTests.Views.Popups.RecipeConfiguration;

[TestFixture]
internal sealed class RecipeConfigurationPopupViewModelTests
{
    private RecipeConfigurationPopupViewModel _sut;
    private Mock<IPopupManagerLight> _popupManager;
    private Mock<IFeedProvider> _feedProvider;
    private List<IVerifyFeed> _selectedFeed;
    private Mock<IFeedVerifySpecificAdapter> _feed1;
    private Mock<IFeedVerifySpecificAdapter> _feed2;
    private Mock<IFeedQuantityAdapter> _quantity1;
    private Mock<IFeedQuantityAdapter> _quantity2;

    [SetUp]
    public void TestInitialize()
    {
        // mock:
        _popupManager = new Mock<IPopupManagerLight>();
        _feedProvider = new Mock<IFeedProvider>();

        _feed1 = CreateFeed("feed1_name");
        _feed2 = CreateFeed("feed2_name");

        _feed1.Setup(o => o.GetUnderlyingFeed()).Returns(new Mock<IFeed>().Object);
        _feed2.Setup(o => o.GetUnderlyingFeed()).Returns(new Mock<IFeed>().Object);

        _quantity1 = new Mock<IFeedQuantityAdapter>();
        _quantity2 = new Mock<IFeedQuantityAdapter>();

        _quantity1.Setup(o => o.Quantity).Returns(60);
        _quantity2.Setup(o => o.Quantity).Returns(20);

        _feed1.Setup(o => o.FeedQuantity).Returns(_quantity1.Object);
        _feed2.Setup(o => o.FeedQuantity).Returns(_quantity2.Object);

        _selectedFeed = new List<IVerifyFeed>
        {
            _feed1.Object,
            _feed2.Object,
        };

        // software under test:
        _sut = new RecipeConfigurationPopupViewModel(_popupManager.Object, _feedProvider.Object, _selectedFeed);
    }

    [Test]
    public void ctor_assign_expectedValues()
    {
        //Arrange

        //Act
        var res = _sut.SelectedFeedsCollection.ToArray<FeedForRecipeCreationAdapter>();

        //Assert
        Assert.AreEqual(2, res.Count);
        Assert.AreEqual("feed1_name", res[0].Name);
        Assert.AreEqual("feed2_name", res[1].Name);
        Assert.AreNotSame(res[0].FeedQuantity, _quantity1.Object, "adapter should have same values but not be the same object");
        Assert.AreNotSame(res[1].FeedQuantity, _quantity2.Object, "adapter should have same values but not be the same object");
        Assert.AreNotSame(60d, res[0].FeedQuantity.Quantity);
        Assert.AreNotSame(20d, res[1].FeedQuantity.Quantity);
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
        var feed1 = CreateFeed("feed1");
        var feed2 = CreateFeed("feed2");
        var feed3 = CreateFeed("feed3");
        var recipe = new Mock<IRecipeAdapter>();
        // the recipe already contains all previous feed except feed3
        recipe.Setup(o => o.Ingredients).Returns(new List<IVerifyFeed> { feed1.Object, feed2.Object });

        var feeds = new List<IFeedThatCouldBeAddedIntoRecipe>
        {
            feed1.Object,
            feed2.Object,
            feed3.Object,
            recipe.Object,
        };

        //Act
        var sut = new RecipeConfigurationPopupViewModel(_popupManager.Object, _feedProvider.Object, feeds);

        //Assert
        var selectedFeeds = sut.SelectedFeedsCollection.ToArray<FeedForRecipeCreationAdapter>();
        Assert.AreEqual(3, selectedFeeds.Count);
        Assert.IsTrue(selectedFeeds.Any(o => o.Name == feed1.Object.Name));
        Assert.IsTrue(selectedFeeds.Any(o => o.Name == feed2.Object.Name));
        Assert.IsTrue(selectedFeeds.Any(o => o.Name == feed3.Object.Name));
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