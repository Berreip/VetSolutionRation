using Moq;
using NUnit.Framework;
using VSR.WPF.Utils.Helpers;
using VSR.WPF.Utils.Services;

namespace VetSolutionRation.wpf.UnitTests.Services.Helpers;

[TestFixture]
internal sealed class SearchFiltersTests
{
    [Test]
    public void SearchFilters_returns_true_when_no_filter()
    {
        //Arrange
        var animal = new Mock<ISearcheable>();
        animal.Setup(o => o.MatchSearch(It.IsAny<string[]>())).Returns(false);

        //Act
        var res = SearchFilters.FilterIngredientsOrRecipe(animal.Object, Array.Empty<string>(), FilterKind.All);

        //Assert
        Assert.IsTrue(res);
    } 
    
    [Test]
    [TestCase(FilterKind.Ingredient)]
    [TestCase(FilterKind.Recipe)]
    public void SearchFilters_returns_false_not_expected_kind(FilterKind filter)
    {
        //Arrange
        var animal = new Mock<ISearcheable>();
        animal.Setup(o => o.MatchSearch(It.IsAny<string[]>())).Returns(false);

        //Act
        var res = SearchFilters.FilterIngredientsOrRecipe(animal.Object, Array.Empty<string>(), filter);

        //Assert
        Assert.IsFalse(res);
    }
}