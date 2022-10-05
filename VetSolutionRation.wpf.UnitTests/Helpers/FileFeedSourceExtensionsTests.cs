using NUnit.Framework;
using VSR.WPF.Utils.Helpers;

namespace VetSolutionRation.wpf.UnitTests.Helpers;

internal sealed class FileFeedSourceExtensionsTests
{
    [Test]
    // En-US
    [TestCase(@"INRA2018_ConcentrateFeedTables_08022018.xlsx", FileFeedSource.Reference)]
    [TestCase(@"INRA2022_ConcentrateFeedTables_08022022.xlsx", FileFeedSource.Reference)]
    [TestCase(@"INRA2018_ForageFeedTables_17042019.xlsx", FileFeedSource.Reference)]
    [TestCase(@"INRA2022_ForageFeedTables_17042022.xlsx", FileFeedSource.Reference)]
    // fr-FR
    [TestCase(@"INRA2018_TablesConcentres_08022018.xlsx", FileFeedSource.Reference)]
    [TestCase(@"INRA2022_TablesConcentres_08022022.xlsx", FileFeedSource.Reference)]
    [TestCase(@"INRA2018_TablesFourrages_17042019.xlsx", FileFeedSource.Reference)]
    [TestCase(@"INRA2022_TablesFourrages_17042022.xlsx", FileFeedSource.Reference)]
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