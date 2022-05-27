using System.Linq;
using NUnit.Framework;
using VetSolutionRationLib.Helpers;

namespace VetSolutionRation.UnitTest.Helpers
{
    [TestFixture]
    public class SearchHelpersTests
    {
        [Test]
        public void SplitByWhitspace_retuns_no_split_when_empty()
        {
            //Arrange

            //Act
            var res = SearchHelpers.SplitByWhitspaceAndSpecificSymbols("");

            //Assert
            Assert.AreEqual(0, res.Length);
        }
        
        [Test]
        public void SplitByWhitspace_retuns_trimmed_split()
        {
            //Arrange

            //Act
            var res = SearchHelpers.SplitByWhitspaceAndSpecificSymbols("   blou    bloub   ");

            //Assert
            Assert.AreEqual(2, res.Length);
            Assert.AreEqual("blou", res[0]);
            Assert.AreEqual("bloub", res[1]);
        }
        
        [Test]
        public void SplitByWhitspace_with_parenthesis()
        {
            //Arrange

            //Act
            var res = SearchHelpers.SplitByWhitspaceAndSpecificSymbols("   (blou)) (    bloub   ");

            //Assert
            Assert.AreEqual(2, res.Length);
            Assert.Contains("blou", res);
            Assert.Contains("bloub", res);
        }
        
        [Test]
        public void SplitByWhitspace_retuns_no_split_when_no_whitespace()
        {
            //Arrange

            //Act
            var res = SearchHelpers.SplitByWhitspaceAndSpecificSymbols("bloublou");

            //Assert
            Assert.AreEqual(1, res.Length);
            Assert.AreEqual("bloublou", res.Single());
        }

        [Test]
        public void SplitByWhitspace_retuns_split_when_whitespace()
        {
            //Arrange

            //Act
            var res = SearchHelpers.SplitByWhitspaceAndSpecificSymbols("blou bloub");

            //Assert
            Assert.AreEqual(2, res.Length);
            Assert.AreEqual("blou", res[0]);
            Assert.AreEqual("bloub", res[1]);
        }
    }
}