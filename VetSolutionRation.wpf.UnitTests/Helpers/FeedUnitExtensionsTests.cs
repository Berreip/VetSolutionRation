using NUnit.Framework;
using VetSolutionRation.wpf.Helpers;
using VetSolutionRationLib.Enums;

namespace VetSolutionRation.wpf.UnitTests.Helpers;

[TestFixture]
internal sealed class FeedUnitExtensionsTests
{
    [Test]
    public void FeedUnitExtensions_ToDisplayName_all_enum_memebers_have_a_value()
    {
        //Arrange

        //Act
        foreach (FeedUnit unit in Enum.GetValues(typeof(FeedUnit)))
        {
            var res = unit.ToDiplayName();
            Assert.IsNotNull(res);
        }

        //Assert
    }

}