using NUnit.Framework;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models;

namespace VetSolutionRation.UnitTest.Models
{
    [TestFixture]
    internal sealed class AnimalTests
    {
        [Test]
        public void Ctor_assigns_default_subkind_value()
        {
            //Arrange

            //Act
            var res = new Animal(AnimalKind.BovineFemale);

            //Assert
            Assert.AreEqual(AnimalKind.BovineFemale, res.Kind);
            Assert.AreEqual(AnimalSubKind.Undefined, res.SubKind);
        }
        
        public void Ctor_assign_correct_values()
        {
            //Arrange

            //Act
            var res = new Animal(AnimalKind.BovineFemale, AnimalSubKind.Heifer);

            //Assert
            Assert.AreEqual(AnimalKind.BovineFemale, res.Kind);
            Assert.AreEqual(AnimalSubKind.Heifer, res.SubKind);
        }
    }
}