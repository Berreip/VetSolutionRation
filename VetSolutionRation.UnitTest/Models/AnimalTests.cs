using NUnit.Framework;
using VSR.Enums;
using VSR.Models;

namespace VetSolutionRation.UnitTest.Models
{
    [TestFixture]
    internal sealed class AnimalTests
    {
        [Test]
        public void Ctor_assign_correct_values()
        {
            //Arrange

            //Act
            var res = new Animal(AnimalKind.Bovine, "bovin");

            //Assert
            Assert.AreEqual(AnimalKind.Bovine, res.Kind);
            Assert.AreEqual("bovin", res.Description);
        }
    }
}