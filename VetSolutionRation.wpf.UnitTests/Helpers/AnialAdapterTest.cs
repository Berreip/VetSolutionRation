using NUnit.Framework;
using VetSolutionRation.wpf.Views.Adapter;
using VetSolutionRationLib.Enums;

namespace VetSolutionRation.wpf.UnitTests.Helpers;

[TestFixture]
internal sealed class AnialAdapterTest
{
    [Test]
    public void Animal_contains_all_returns_true_when_all_contains()
    {
        //Arrange
        var sut = new AnimalAdapter(AnimalKind.Bovine, "Taureau toto toutou tata");

        //Act
        var res = sut.MatchSearch(new[]
        {
            "Taureau",
            "toto",
            "toutou",
            "tata",
        });

        //Assert
        Assert.IsTrue(res);
    }
}