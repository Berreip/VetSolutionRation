using NUnit.Framework;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Adapters;

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
        var feed = new FeedAdapter(adapterName);

        //Act
        var res = feed.MatchSearch(new[] { searchText });

        //Assert
        Assert.AreEqual(expectedMatch, res);
    }
}