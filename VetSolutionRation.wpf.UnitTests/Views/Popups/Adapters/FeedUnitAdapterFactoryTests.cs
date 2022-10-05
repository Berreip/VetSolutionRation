using Moq;
using NUnit.Framework;
using VSR.Models.Units;
using VSR.WPF.Utils.Adapters.EditionIngredients;

namespace VetSolutionRation.wpf.UnitTests.Views.Popups.Adapters;

[TestFixture]
internal sealed class FeedUnitAdapterFactoryTests
{
    [Test]
    public void CreateUnit_returns_expected_type_for_PercentageUnit()
    {
        //Arrange
        var unit = new Mock<IPercentageUnit>();

        //Act
        var res = FeedUnitAdapterFactory.CreateUnit(unit.Object);

        //Assert
        Assert.IsTrue(res is PercentageUnitFeedAdapter);
    } 
    
    [Test]
    public void CreateUnit_returns_expected_type_for_SpecificUnit()
    {
        //Arrange
        var unit = new Mock<ISpecificUnit>();

        //Act
        var res = FeedUnitAdapterFactory.CreateUnit(unit.Object);

        //Assert
        Assert.IsTrue(res is SpecificUnitFeedAdapter);
    } 
    
    [Test]
    public void CreateUnit_returns_expected_type_for_NoUnit()
    {
        //Arrange
        var unit = new NoUnit();

        //Act
        var res = FeedUnitAdapterFactory.CreateUnit(unit);

        //Assert
        Assert.IsTrue(res is NoUnitFeedAdapter);
    }

    [Test]
    public void PercentageUnitFeedAdapter_returns_display_label_with_mention_if_specified()
    {
        //Arrange
        var unit = new PercentageUnit("foo %");

        //Act
        var res = FeedUnitAdapterFactory.CreateUnit(unit);

        //Assert
        Assert.AreEqual("foo %", res.DisplayLabel);
    }
    
    [Test]
    public void PercentageUnitFeedAdapter_returns_default_percentage_if_no_label_specified()
    {
        //Arrange
        var unit = new PercentageUnit();

        //Act
        var res = FeedUnitAdapterFactory.CreateUnit(unit);

        //Assert
        Assert.AreEqual("%", res.DisplayLabel);
    }

    [Test]
    public void SpecificUnitFeedAdapter_returns_display_label_with_mention()
    {
        //Arrange
        var unit = Units.GKgMs;

        //Act
        var res = FeedUnitAdapterFactory.CreateUnit(unit);

        //Assert
        Assert.AreEqual(@"g/kg MS", res.DisplayLabel);
    }
}