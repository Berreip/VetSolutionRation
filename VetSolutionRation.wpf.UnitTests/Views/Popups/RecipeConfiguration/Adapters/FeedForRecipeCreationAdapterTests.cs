using Moq;
using NUnit.Framework;
using VetSolutionRation.wpf.Views.Adapter;
using VetSolutionRation.wpf.Views.Popups.RecipeConfiguration.Adapters;
using VetSolutionRationLib.Enums;

namespace VetSolutionRation.wpf.UnitTests.Views.Popups.RecipeConfiguration.Adapters;

[TestFixture]
internal sealed class FeedForRecipeCreationAdapterTests
{
    private FeedForRecipeCreationAdapter _sut;
    private Mock<IVerifyFeed> _feed;

    [SetUp]
    public void TestInitialize()
    {
        // mock:
        _feed = new Mock<IVerifyFeed>();
        _feed.Setup(o => o.Name).Returns("foo_name");
        var feedQty = new Mock<IFeedQuantityAdapter>();
        feedQty.Setup(o => o.Quantity).Returns(89);
        feedQty.Setup(o => o.Unit).Returns(FeedUnit.Kg);
        _feed.Setup(o => o.FeedQuantity).Returns(feedQty.Object);
        
        // software under test:
        _sut = new FeedForRecipeCreationAdapter(_feed.Object);
    }

    [Test]
    public void Ctor_assign_expected_name()
    {
        //Arrange

        //Act
        var res = _sut.Name;

        //Assert
        Assert.AreEqual(_feed.Object.Name, res);
    }

    /// <summary>
    /// As FeedQuantity is mutable, it should not be used as a reference. Otherwise, the same reference could be shared and updated by multiples elements
    /// </summary>
    [Test]
    public void Ctor_create_another_reference_of_feed_quantity_instread_of_using_the_same_reference()
    {
        //Arrange

        //Act
        var res = _sut.FeedQuantity;

        //Assert
        Assert.AreNotSame(_feed.Object.FeedQuantity, res);
    }
    
    [Test]
    public void Ctor_assign_expected_Feed_Qty()
    {
        //Arrange

        //Act
        var res = _sut.FeedQuantity;

        //Assert
        Assert.AreEqual(_feed.Object.FeedQuantity.Quantity, res.Quantity);
        Assert.AreEqual(_feed.Object.FeedQuantity.Unit, res.Unit);
    }

}