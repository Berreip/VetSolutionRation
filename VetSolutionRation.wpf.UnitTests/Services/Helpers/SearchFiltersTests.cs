using Moq;
using NUnit.Framework;
using VetSolutionRation.wpf.Services.Helpers;
using VetSolutionRation.wpf.Views.RatioPanel.Adapter;

namespace VetSolutionRation.wpf.UnitTests.Services.Helpers;

[TestFixture]
internal sealed class SearchFiltersTests
{
    [Test]
    public void SearchFilters_returns_true_when_no_filter()
    {
        //Arrange
        var animal = new Mock<ISearcheable>();
        animal.Setup(o => o.ContainsAll(It.IsAny<string[]>())).Returns(false);

        //Act
        var res = SearchFilters.FilterParts(animal.Object, Array.Empty<string>());

        //Assert
        Assert.IsTrue(res);
    }
}