using Moq;
using NUnit.Framework;
using VetSolutionRation.wpf.Services.Feed;
using VetSolutionRation.wpf.Services.PopupManager;
using VetSolutionRation.wpf.UnitTests.UnitTestUtils;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Adapters;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Verify;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.Recipe;
using VetSolutionRationLib.Enums;

namespace VetSolutionRation.wpf.UnitTests.Views;

[TestFixture]
internal sealed class VerifyRatiosViewModelTests
{
    private VerifyRatiosViewModel _sut;
    private Mock<IRecipeCalculator> _recipeCalculator;
    private Mock<IFeedProvider> _feedProvider;
    private Mock<IPopupManagerLight> _popupManagerLight;

    [SetUp]
    public void TestInitialize()
    {
        // mock:
        _recipeCalculator = new Mock<IRecipeCalculator>();
        _feedProvider = new Mock<IFeedProvider>();
        _popupManagerLight = new Mock<IPopupManagerLight>();

        // software under test:
        _sut = new VerifyRatiosViewModel(_recipeCalculator.Object, _feedProvider.Object, _popupManagerLight.Object);
        
        // bind can execute:
        _sut.CreateRecipeCommand.BindCanExecuteChangedForUnitTests();
        _sut.RemoveFromSelectedFeedsCommand.BindCanExecuteChangedForUnitTests();
    }

    [Test]
    public void ctor_assign_correct_initial_value()
    {
        //Arrange

        //Act

        //Assert
        Assert.IsFalse(_sut.CreateRecipeCommand.CanExecute());
        Assert.IsEmpty(_sut.SelectedFeedsForVerifyPanel.ToArray<IVerifyFeed>());
    }

    [Test]
    [TestCase(true)]
    [TestCase(false)]
    public void CreateRecipeCommand_can_execute_evaluated_by_calculator(bool recipeCalculatorResult)
    {
        //Arrange
        _recipeCalculator
            .Setup(o => o.CouldCalculateRecipe(It.IsAny<IReadOnlyCollection<IFeedThatCouldBeAddedIntoReciepe>>()))
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
        var feed = new Mock<IFeedAdapter>();

        //Act
        _sut.AddSelectedFeed(feed.Object);

        //Assert
        _recipeCalculator.Verify(o => o.CouldCalculateRecipe(It.IsAny<IReadOnlyCollection<IFeedThatCouldBeAddedIntoReciepe>>()), Times.Once);
    }
    
      
    [Test]
    public void CreateRecipeCommand_can_execute_evaluated_if_feed_selection_changed()
    {
        //Arrange
        var feed = new Mock<IFeedAdapter>();
        _sut.AddSelectedFeed(feed.Object);
        var feedAdapter = _sut.SelectedFeedsForVerifyPanel.ToArray<FeedVerifySpecificAdapter>().Single();
        
        // reset the mock to count calls from here
        _recipeCalculator.Reset();
        
        //Act
        feedAdapter.IsSelected = false;
        
        //Assert
        _recipeCalculator.Verify(o => o.CouldCalculateRecipe(It.IsAny<IReadOnlyCollection<IFeedThatCouldBeAddedIntoReciepe>>()), Times.Once);
    }
    
    [Test]
    public void CreateRecipeCommand_can_execute_evaluated_if_feed_is_removed()
    {
        //Arrange
        var feed = new Mock<IFeedAdapter>();
        _sut.AddSelectedFeed(feed.Object);
        var feedAdapter = _sut.SelectedFeedsForVerifyPanel.ToArray<FeedVerifySpecificAdapter>().Single();
        
        // reset the mock to count calls from here
        _recipeCalculator.Reset();

        //Act
        _sut.RemoveFromSelectedFeedsCommand.Execute(feedAdapter);

        //Assert
        _recipeCalculator.Verify(o => o.CouldCalculateRecipe(It.IsAny<IReadOnlyCollection<IFeedThatCouldBeAddedIntoReciepe>>()), Times.Once);
    }

    [Test]
    public void AddSelectedFeed_add_once_when_not_known_before()
    {
        //Arrange
        var feed = new Mock<IFeedAdapter>();

        //Act
        _sut.AddSelectedFeed(feed.Object);

        //Assert
        var verifyFeeds = _sut.SelectedFeedsForVerifyPanel.ToArray<IVerifyFeed>();
        Assert.AreEqual(1, verifyFeeds.Count);
        Assert.AreSame(feed.Object, verifyFeeds[0].GetUnderlyingFeedAdapter());
    }
    
    [Test]
    public void AddSelectedFeed_add_once_when_duplicated()
    {
        //Arrange
        var feed = new Mock<IFeedAdapter>();

        //Act
        for (var i = 0; i < 10; i++)
        {
            _sut.AddSelectedFeed(feed.Object);
        }

        //Assert
        var verifyFeeds = _sut.SelectedFeedsForVerifyPanel.ToArray<IVerifyFeed>();
        Assert.AreEqual(1, verifyFeeds.Count);
        Assert.AreSame(feed.Object, verifyFeeds[0].GetUnderlyingFeedAdapter());
    }
    
    [Test]
    public void AddSelectedFeed_add_recipe()
    {
        //Arrange
        var recipe = new Mock<IRecipeAdapter>();

        //Act
        _sut.AddSelectedFeed(recipe.Object);

        //Assert
        var verifyFeeds = _sut.SelectedFeedsForVerifyPanel.ToArray<IVerifyFeed>();
        Assert.AreEqual(1, verifyFeeds.Count);
        Assert.AreSame(recipe.Object, verifyFeeds[0].GetUnderlyingFeedAdapter());
    }

}