using Moq;
using VetSolutionRation.wpf.Views.Adapter;
using VetSolutionRationLib.Enums;

namespace VetSolutionRation.wpf.UnitTests.Views.Popups.RecipeConfiguration;

internal static class UtilsUnitTests
{
    public static Mock<IFeedVerifySpecificAdapter> CreateFeed(string name)
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

    public static Mock<IIngredientFeedAdapter> CreateIngredientFeed(string name)
    {
        var feed = new Mock<IIngredientFeedAdapter>();
        feed.Setup(o => o.Name).Returns(name);
        feed.Setup(o => o.Guid).Returns(Guid.NewGuid());
        var qty = new Mock<IFeedQuantityAdapter>();
        qty.Setup(o => o.Unit).Returns(FeedUnit.Kg);
        qty.Setup(o => o.Quantity).Returns(1);
        feed.Setup(o => o.FeedQuantity).Returns(qty.Object);
        return feed;
    }
}