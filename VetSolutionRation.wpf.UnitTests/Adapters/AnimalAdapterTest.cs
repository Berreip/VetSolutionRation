using NUnit.Framework;
using VetSolutionRation.wpf.EnumExtensions;
using VetSolutionRation.wpf.Views.RatioPanel.Adapter;
using VetSolutionRationLib.Enums;

namespace VetSolutionRation.wpf.UnitTests.Adapters
{
    [TestFixture]
    internal sealed class AnimalAdapterTest
    {
        [Test]
        public void ContainsAny_retuns_true_if_empty()
        {
            //Arrange
            var sut = new AnimalAdapter(AnimalKind.BovineFemale, AnimalSubKind.Heifer, "1/2 ans et autre chose");

            //Act
            var res = sut.MatchSearch(Array.Empty<string>());

            //Assert
            Assert.IsTrue(res);
        }

        [Test]
        public void ContainsAny_retuns_true_if_match()
        {
            //Arrange
            var sut = new AnimalAdapter(AnimalKind.BovineFemale, AnimalSubKind.Heifer, "1/2 ans");

            //Act
            var res = sut.MatchSearch(new []{"1/2"});

            //Assert
            Assert.IsTrue(res);
        }

        [Test]
        public void ContainsAny_retuns_true_if_match_for_animal_kind()
        {
            //Arrange
            var sut = new AnimalAdapter(AnimalKind.BovineFemale, AnimalSubKind.Heifer, "1/2 ans");

            //Act
            var res = sut.MatchSearch(new [] { AnimalKind.BovineFemale.GetDisplayName()[..2] });

            //Assert
            Assert.IsTrue(res);
        }

        [Test]
        public void ContainsAny_retuns_true_if_match_for_animal_subkind()
        {
            //Arrange
            var sut = new AnimalAdapter(AnimalKind.BovineFemale, AnimalSubKind.Heifer, "1/2 ans");

            //Act
            var res = sut.MatchSearch(new [] { AnimalSubKind.Heifer.GetDisplayName()[..4] });

            //Assert
            Assert.IsTrue(res);
        }
        
        [Test]
        public void ContainsAny_retuns_true_if_multiple_match()
        {
            //Arrange
            var sut = new AnimalAdapter(AnimalKind.BovineFemale, AnimalSubKind.Heifer, "1/2 ans");

            //Act
            var res = sut.MatchSearch(new [] { AnimalSubKind.Heifer.GetDisplayName()[..4], "1/2" });

            //Assert
            Assert.IsTrue(res);
        }
        
        [Test]
        public void ContainsAny_retuns_false_if_one_match_only_among_many()
        {
            //Arrange
            var sut = new AnimalAdapter(AnimalKind.BovineFemale, AnimalSubKind.Heifer, "1/2 ans");

            //Act
            var res = sut.MatchSearch(new [] { AnimalSubKind.Heifer.GetDisplayName()[..4], "foo" });

            //Assert
            Assert.IsFalse(res);
        }
        
        [Test]
        public void ContainsAny_retuns_false_if_no_match()
        {
            //Arrange
            var sut = new AnimalAdapter(AnimalKind.BovineFemale, AnimalSubKind.Heifer, "1/2 ans");

            //Act
            var res = sut.MatchSearch(new []{"foooooooooo"});

            //Assert
            Assert.IsFalse(res);
        }  
        
        [Test]
        public void ContainsAny_retuns_false_if_no_match_for_multiple_times()
        {
            //Arrange
            var sut = new AnimalAdapter(AnimalKind.BovineFemale, AnimalSubKind.Heifer, "1/2 ans");

            //Act
            var res = sut.MatchSearch(new []{"1/2", "1/2"});

            //Assert
            Assert.IsFalse(res);
        }
        
        [Test]
        public void ContainsAny_retuns_false_if_match_for_multiple_times()
        {
            //Arrange
            var sut = new AnimalAdapter(AnimalKind.BovineFemale, AnimalSubKind.Heifer, AnimalSubKind.Heifer.GetDisplayName());

            //Act
            var res = sut.MatchSearch(new []{AnimalSubKind.Heifer.GetDisplayName(), AnimalSubKind.Heifer.GetDisplayName()});

            //Assert
            Assert.IsTrue(res);
        }

        [Test]
        [TestCase(AnimalKind.BovineFemale, AnimalSubKind.Heifer, "toto tata", "[kindkey] | [subkindkey] | toto tata")]
        [TestCase(AnimalKind.BovineFemale, AnimalSubKind.Heifer, "", "[kindkey] | [subkindkey]")]
        [TestCase(AnimalKind.BovineFemale, AnimalSubKind.Undefined, "", "[kindkey]")]
        [TestCase(AnimalKind.BovineFemale, AnimalSubKind.Undefined, "toto tata", "[kindkey] | toto tata")]
        public void ToString_return_expected_result(AnimalKind kind, AnimalSubKind subkind, string details, string expectedResult)
        {
            //Arrange
            var refResult = expectedResult
                .Replace("[kindkey]", kind.GetDisplayName())
                .Replace("[subkindkey]", subkind.GetDisplayName());

            //Act
            var res = new AnimalAdapter(kind, subkind, details).ToString();

            //Assert
            Assert.AreEqual(refResult, res);
        }
    }
}