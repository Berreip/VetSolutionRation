using NUnit.Framework;
using VSR.Enums;
using VSR.WPF.Utils.Helpers;

namespace VetSolutionRation.wpf.UnitTests.Helpers;

[TestFixture]
internal sealed class FeedUnitWpfExtensionsTests
{
    [Test]
    public void ToDisplayName_all_enum_memebers_have_a_value()
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