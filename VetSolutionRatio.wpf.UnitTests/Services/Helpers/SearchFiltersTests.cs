using Moq;
using NUnit.Framework;
using VetSolutionRatio.wpf.Services.Helpers;
using VetSolutionRatio.wpf.Views.RatioPanel.Adapter;

namespace VetSolutionRatio.wpf.UnitTests.Services.Helpers;

[TestFixture]
internal sealed class SearchFiltersTests
{
    [Test]
    public void SearchFilters_returns_true_when_no_filter()
    {
        //Arrange
        var animal = new Mock<IAnimalKindAdapter>();
        animal.Setup(o => o.ContainsAll(It.IsAny<string[]>())).Returns(false);

        //Act
        var res = SearchFilters.FilterAnimalKind(animal.Object, Array.Empty<string>());

        //Assert
        Assert.IsTrue(res);
    }
}