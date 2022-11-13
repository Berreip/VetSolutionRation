using NUnit.Framework;
using VSR.WPF.Utils.Adapters.CalculationAdapters;

namespace VetSolutionRation.wpf.UnitTests.Adapters;

[TestFixture]
internal sealed class FeedQuantityAdapterTests
{
    private IngredientOrRecipeQuantityAdapter _sut;

    [SetUp]
    public void TestInitialize()
    {
        // mock:

        // software under test:
        _sut = new IngredientOrRecipeQuantityAdapter();
    }

    [Test]
    public void ctor_assign_correct_values()
    {
        //Arrange

        //Act

        //Assert
        Assert.AreEqual(1, _sut.Quantity);
        Assert.AreEqual("1", _sut.QuantityString);
        Assert.AreEqual(true, _sut.IsValid);
        Assert.AreEqual("kg", _sut.UnitDisplayName);
    }
}