using NUnit.Framework;
using VetSolutionRation.wpf.EnumExtensions;
using VetSolutionRationLib.Enums;

namespace VetSolutionRation.wpf.UnitTests.EnumsExtensions;

[TestFixture]
internal sealed class AnimalSubKindExtensionsTests
{
    [Test]
    public void AnimalSubKindExtensions_assert_that_all_value_returns_a_display_name()
    {
        //Arrange

        //Act
        foreach (AnimalSubKind kind in Enum.GetValues(typeof(AnimalSubKind)))
        {
            //Assert
            Assert.DoesNotThrow(() => kind.GetDisplayName());
        }
    }
}