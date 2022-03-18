﻿using NUnit.Framework;
using VetSolutionRatio.wpf.EnumExtensions;
using VetSolutionRatioLib.Enums;

namespace VetSolutionRatio.wpf.UnitTests.EnumsExtensions
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