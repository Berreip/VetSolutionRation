using NUnit.Framework;
using VetSolutionRation.wpf.Views.IngredientsAndRecipesList.Adapters;
using VSR.WPF.Utils.Services;

namespace VetSolutionRation.wpf.UnitTests.Adapters;

[TestFixture]
internal sealed class FilterKindAdapterTests
{
    private FilterKindAdapter _sut;

    [SetUp]
    public void TestInitialize()
    {
        // mock:

        // software under test:
        _sut = new FilterKindAdapter(FilterKind.Recipe);
    }

    [Test]
    public void DisplayText_returns_expected_Value()
    {
        //Arrange

        //Act
        var res = _sut.DisplayText;

        //Assert
        Assert.AreEqual("Recette", res);
    }

    [Test]
    public void Kind_returns_expected_Value()
    {
        //Arrange

        //Act
        var res = _sut.Kind;

        //Assert
        Assert.AreEqual(FilterKind.Recipe, res);
    }

}