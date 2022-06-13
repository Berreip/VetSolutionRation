using Moq;
using NUnit.Framework;
using VetSolutionRation.wpf.Services.Feed;
using VetSolutionRation.wpf.Services.PopupManager;
using VetSolutionRation.wpf.UnitTests.UnitTestUtils;
using VetSolutionRation.wpf.Views.Popups.RecipeConfiguration;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Adapters;

namespace VetSolutionRation.wpf.UnitTests.Views.Popups.RecipeConfiguration;

[TestFixture]
internal sealed class RecipeConfigurationPopupViewModelTests
{
    private RecipeConfigurationPopupViewModel _sut;
    private Mock<IPopupManagerLight> _popupManager;
    private Mock<IFeedProvider> _feedProvider;
    private List<IVerifyFeed> _selectedFeed;
    private Mock<IVerifyFeed> _feed1;
    private Mock<IVerifyFeed> _feed2;
    private Mock<IFeedQuantityAdapter> _quantity1;
    private Mock<IFeedQuantityAdapter> _quantity2;

    [SetUp]
    public void TestInitialize()
    {
        // mock:
        _popupManager = new Mock<IPopupManagerLight>();
        _feedProvider = new Mock<IFeedProvider>();
        
        _feed1 = new Mock<IVerifyFeed>();
        _feed2 = new Mock<IVerifyFeed>();

        _feed1.Setup(o => o.GetUnderlyingFeedAdapter()).Returns(new Mock<IFeedAdapter>().Object);
        _feed2.Setup(o => o.GetUnderlyingFeedAdapter()).Returns(new Mock<IFeedAdapter>().Object);

        _feed1.Setup(o => o.Name).Returns("feed1_name");
        _feed2.Setup(o => o.Name).Returns("feed2_name");
        
        _quantity1 = new Mock<IFeedQuantityAdapter>();
        _quantity2 = new Mock<IFeedQuantityAdapter>();

        _quantity1.Setup(o => o.Quantity).Returns(60d);
        _quantity2.Setup(o => o.Quantity).Returns(20d);

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
        _sut.ReciepeName = "stubName";

        //Assert
        Assert.IsTrue(_sut.ValidateRecipeCreationCommand.CanExecute());
    }

    [Test]
    public void Execute_validate_setup_recipe_configuration()
    {
        //Arrange
        _sut.ReciepeName = "stubName";

        //Act
        _sut.ValidateRecipeCreationCommand.Execute();
        
        //Assert
        var config = _sut.ReciepedConfiguration;
        Assert.IsNotNull(config);
        
        var ingredients = config.GetIngredients();
        Assert.AreEqual(2, ingredients.Count);
        Assert.AreEqual(0.75d, ingredients[0].Percentage);
        Assert.AreEqual(0.25d, ingredients[1].Percentage);
        
    }

    

}