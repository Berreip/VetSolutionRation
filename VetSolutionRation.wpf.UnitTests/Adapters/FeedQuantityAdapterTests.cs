using NUnit.Framework;
using VetSolutionRation.wpf.Views.Adapter;
using VetSolutionRationLib.Enums;

namespace VetSolutionRation.wpf.UnitTests.Adapters;

[TestFixture]
internal sealed class FeedQuantityAdapterTests
{
    private FeedQuantityAdapter _sut;

    [SetUp]
    public void TestInitialize()
    {
        // mock:

        // software under test:
        _sut = new FeedQuantityAdapter(FeedUnit.Kg);
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
        Assert.AreEqual(FeedUnit.Kg, _sut.Unit);
    }
}