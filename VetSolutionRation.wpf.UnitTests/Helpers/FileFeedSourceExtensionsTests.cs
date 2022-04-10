using NUnit.Framework;
using VetSolutionRation.wpf.Helpers;

namespace VetSolutionRation.wpf.UnitTests.Helpers;

internal sealed class FileFeedSourceExtensionsTests
{
    [Test]
    // En-US
    [TestCase(@"INRA2018_ConcentrateFeedTables_08022018.xlsx", FileFeedSource.Concentrate)]
    [TestCase(@"INRA2022_ConcentrateFeedTables_08022022.xlsx", FileFeedSource.Concentrate)]
    [TestCase(@"INRA2018_ForageFeedTables_17042019.xlsx", FileFeedSource.Forage)]
    [TestCase(@"INRA2022_ForageFeedTables_17042022.xlsx", FileFeedSource.Forage)]
    // fr-FR
    [TestCase(@"INRA2018_TablesConcentres_08022018.xlsx", FileFeedSource.Concentrate)]
    [TestCase(@"INRA2022_TablesConcentres_08022022.xlsx", FileFeedSource.Concentrate)]
    [TestCase(@"INRA2018_TablesFourrages_17042019.xlsx", FileFeedSource.Forage)]
    [TestCase(@"INRA2022_TablesFourrages_17042022.xlsx", FileFeedSource.Forage)]
    // failed
    [TestCase(@"file.xlsx", null)]
    [TestCase(@"", null)]
    [TestCase(null, null)]
    public void TryParseFromFileName_returns_expected_result(string nameToParse, FileFeedSource? expectedFeedSource)
    {
        //Arrange

        //Act
        var res = FileFeedSourceExtensions.TryParseFromFileName(nameToParse, out var parsed);

        //Assert
        if (expectedFeedSource.HasValue)
        {
            Assert.IsTrue(res);
            Assert.AreEqual(expectedFeedSource.Value, parsed);
        }
        else
        {
            Assert.IsFalse(res);
        }

    }

}