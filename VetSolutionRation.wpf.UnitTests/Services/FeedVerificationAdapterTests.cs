using NUnit.Framework;
using VSR.Models.Ingredients;
using VSR.WPF.Utils.Adapters.IngredientsAndRecipeList;

namespace VetSolutionRation.wpf.UnitTests.Services;

[TestFixture]
internal sealed class FeedVerificationAdapterTests
{
    [Test]
    [TestCase("maïs", "maïs", true)]
    [TestCase("maïs", "MAÏS", true)]
    [TestCase("maïs", "MA", true)]
    [TestCase("maïs", "MAis", true)]
    [TestCase("Céreale", "Cereale", true)]
    [TestCase("Céreale", "Ceréale", true)]
    public void SearchReturns_expected_results(string adapterName, string searchText, bool expectedMatch)
    {
        //Arrange
        var feed = new ReferenceIngredientForListAdapter(new Ingredient(Guid.NewGuid(), adapterName, false , Array.Empty<INutritionalDetails>()));

        //Act
        var res = feed.MatchSearch(new[] { searchText });

        //Assert
        Assert.AreEqual(expectedMatch, res);
    }
}