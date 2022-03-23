using NUnit.Framework;
using VetSolutionRation.wpf.EnumExtensions;
using VetSolutionRationLib.Enums;

namespace VetSolutionRation.wpf.UnitTests.EnumsExtensions
{
    [TestFixture]
    internal sealed class AnimalKindExtensionsTests
    {
        [Test]
        public void AnimalKindExtensions_assert_that_all_value_returns_a_display_name()
        {
            //Arrange

            //Act
            foreach (AnimalKind kind in Enum.GetValues(typeof(AnimalKind)))
            {
                var res = kind.GetDisplayName();

                //Assert
                Assert.IsNotEmpty(res);
            }
        }
    }
}