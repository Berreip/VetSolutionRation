using NUnit.Framework;
using VetSolutionRatioLib.Enums;
using VetSolutionRatioLib.Models;

namespace VetSolutionRatio.UnitTest.Models
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