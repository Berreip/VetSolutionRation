using NUnit.Framework;
using VetSolutionRatio.wpf.EnumExtensions;
using VetSolutionRatio.wpf.Views.RatioPanel.Adapter;
using VetSolutionRatioLib.Enums;

namespace VetSolutionRatio.wpf.UnitTests.Adapters
{
    [TestFixture]
    internal sealed class AnimalKindAdapterTest
    {
        [Test]
        public void ContainsAny_retuns_true_if_empty()
        {
            //Arrange
            var sut = new AnimalKindAdapter(AnimalKind.Cow, AnimalSubKind.Heifer, "1/2 ans");

            //Act
            var res = sut.ContainsAll(Array.Empty<string>());

            //Assert
            Assert.IsTrue(res);
        }

        [Test]
        public void ContainsAny_retuns_true_if_match()
        {
            //Arrange
            var sut = new AnimalKindAdapter(AnimalKind.Cow, AnimalSubKind.Heifer, "1/2 ans");

            //Act
            var res = sut.ContainsAll(new []{"1/2"});

            //Assert
            Assert.IsTrue(res);
        }

        [Test]
        public void ContainsAny_retuns_true_if_match_for_animal_kind()
        {
            //Arrange
            var sut = new AnimalKindAdapter(AnimalKind.Cow, AnimalSubKind.Heifer, "1/2 ans");

            //Act
            var res = sut.ContainsAll(new [] { AnimalKind.Cow.GetDisplayName()[..2] });

            //Assert
            Assert.IsTrue(res);
        }

        [Test]
        public void ContainsAny_retuns_true_if_match_for_animal_subkind()
        {
            //Arrange
            var sut = new AnimalKindAdapter(AnimalKind.Cow, AnimalSubKind.Heifer, "1/2 ans");

            //Act
            var res = sut.ContainsAll(new [] { AnimalSubKind.Heifer.GetDisplayName()[..4] });

            //Assert
            Assert.IsTrue(res);
        }
        
        [Test]
        public void ContainsAny_retuns_true_if_multiple_match()
        {
            //Arrange
            var sut = new AnimalKindAdapter(AnimalKind.Cow, AnimalSubKind.Heifer, "1/2 ans");

            //Act
            var res = sut.ContainsAll(new [] { AnimalSubKind.Heifer.GetDisplayName()[..4], "1/2" });

            //Assert
            Assert.IsTrue(res);
        }
        
        [Test]
        public void ContainsAny_retuns_false_if_one_match_only_among_many()
        {
            //Arrange
            var sut = new AnimalKindAdapter(AnimalKind.Cow, AnimalSubKind.Heifer, "1/2 ans");

            //Act
            var res = sut.ContainsAll(new [] { AnimalSubKind.Heifer.GetDisplayName()[..4], "foo" });

            //Assert
            Assert.IsFalse(res);
        }
        
        [Test]
        public void ContainsAny_retuns_false_if_no_match()
        {
            //Arrange
            var sut = new AnimalKindAdapter(AnimalKind.Cow, AnimalSubKind.Heifer, "1/2 ans");

            //Act
            var res = sut.ContainsAll(new []{"foooooooooo"});

            //Assert
            Assert.IsFalse(res);
        }  
        
        [Test]
        public void ContainsAny_retuns_false_if_no_match_for_multiple_times()
        {
            //Arrange
            var sut = new AnimalKindAdapter(AnimalKind.Cow, AnimalSubKind.Heifer, "1/2 ans");

            //Act
            var res = sut.ContainsAll(new []{"1/2", "1/2"});

            //Assert
            Assert.IsFalse(res);
        }
        
        [Test]
        public void ContainsAny_retuns_false_if_match_for_multiple_times()
        {
            //Arrange
            var sut = new AnimalKindAdapter(AnimalKind.Cow, AnimalSubKind.Heifer, AnimalSubKind.Heifer.GetDisplayName());

            //Act
            var res = sut.ContainsAll(new []{AnimalSubKind.Heifer.GetDisplayName(), AnimalSubKind.Heifer.GetDisplayName()});

            //Assert
            Assert.IsTrue(res);
        }
    }
}