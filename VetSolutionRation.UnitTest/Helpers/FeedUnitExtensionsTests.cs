using System;
using NUnit.Framework;
using VetSolutionRationLib.Enums;

namespace VetSolutionRation.UnitTest.Helpers
{
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
                var res = FeedUnitExtensions.ParseFromReferenceLabel(unit.ToReferenceLabel());
                Assert.AreEqual(res, unit);
            }

            //Assert
        }
    }
}

