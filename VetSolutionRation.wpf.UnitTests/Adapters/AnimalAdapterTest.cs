using NUnit.Framework;
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
            var sut = new AnimalAdapter("1/2 ans et autre chose");

            //Act
            var res = sut.MatchSearch(Array.Empty<string>());

            //Assert
            Assert.IsTrue(res);
        }

        [Test]
        public void ContainsAny_retuns_true_if_match()
        {
            //Arrange
            var sut = new AnimalAdapter("bovin 1/2 ans");

            //Act
            var res = sut.MatchSearch(new []{"1/2"});

            //Assert
            Assert.IsTrue(res);
        }

        [Test]
        public void ContainsAny_retuns_true_if_match_for_animal_kind()
        {
            //Arrange
            var sut = new AnimalAdapter("bovin 1/2 ans");

            //Act
            var res = sut.MatchSearch(new [] { "bovin" });

            //Assert
            Assert.IsTrue(res);
        }
        
        [Test]
        public void ContainsAny_retuns_true_if_multiple_match()
        {
            //Arrange
            var sut = new AnimalAdapter("bovin 1/2 ans");

            //Act
            var res = sut.MatchSearch(new [] { "1/2" });

            //Assert
            Assert.IsTrue(res);
        }
        
        [Test]
        public void ContainsAny_retuns_false_if_one_match_only_among_many()
        {
            //Arrange
            var sut = new AnimalAdapter("bovin 1/2 ans");

            //Act
            var res = sut.MatchSearch(new [] { "ovin", "foo" });

            //Assert
            Assert.IsFalse(res);
        }
        
        [Test]
        public void ContainsAny_retuns_false_if_no_match()
        {
            //Arrange
            var sut = new AnimalAdapter("bovin 1/2 ans");

            //Act
            var res = sut.MatchSearch(new []{"foooooooooo"});

            //Assert
            Assert.IsFalse(res);
        }  
        
        [Test]
        public void ContainsAny_retuns_false_if_no_match_for_multiple_times()
        {
            //Arrange
            var sut = new AnimalAdapter("bovin 1/2 ans");

            //Act
            var res = sut.MatchSearch(new []{"1/2", "1/2"});

            //Assert
            Assert.IsFalse(res);
        }
        
        [Test]
        public void ContainsAny_retuns_false_if_match_for_multiple_times()
        {
            //Arrange
            var sut = new AnimalAdapter("génisse génisse");

            //Act
            var res = sut.MatchSearch(new []{"génisse", "génisse"});

            //Assert
            Assert.IsTrue(res);
        }
        
        [Test]
        [TestCase("oto")]
        [TestCase("ot")]
        [TestCase("o")]
        public void MatchSearch_returns_false_if_not_starting_by(string searchText)
        {
            //Arrange
            var sut = new AnimalAdapter("génisse toto");
            

            //Act
            var res = sut.MatchSearch(new []{searchText});

            //Assert
            Assert.IsFalse(res);
        }

        [Test]
        public void MatchSearch_returns_true_if_all_search_starts_with_input()
        {
            //Arrange
            var sut = new AnimalAdapter("toto tata toutou");
            

            //Act
            var res = sut.MatchSearch(new []{"tot", "tat", "tout"});

            //Assert
            Assert.IsTrue(res);
        }

        [Test]
        public void MatchSearch_returns_false_if_searchable_item_contains_less_matches_than_requested_input()
        {
            //Arrange
            var sut = new AnimalAdapter("toto toto ototo");
            

            //Act
            //We ask three times for "toto"'s beginning
            var res = sut.MatchSearch(new []{"tot", "tot", "tot"});

            //Assert
            Assert.IsFalse(res);
        }

    }
}