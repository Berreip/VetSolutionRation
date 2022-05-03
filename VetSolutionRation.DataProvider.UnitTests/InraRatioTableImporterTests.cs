using System;
using System.Linq;
using NUnit.Framework;
using VetSolutionRation.DataProvider.UnitTests.Data;

namespace VetSolutionRation.DataProvider.UnitTests;

[TestFixture]
internal sealed class Tests
{
    [Test]
    [TestCase(AvailableFile.InraRatioConcentrateTable_FR)]
    [TestCase(AvailableFile.InraRatioConcentrateTable_EN)]
    public void ImportInraTable_return_not_null_dto_for_concentrate(AvailableFile concentrateFile)
    {
        //Arrange
        var file = FileGetter.GetFile(concentrateFile);

        //Act
        var res = InraRatioTableImporter.ImportInraTable(file);

        //Assert
        Assert.IsNotNull(res);
    }

    [Test]
    [TestCase(AvailableFile.InraRatioForageTable_FR)]
    [TestCase(AvailableFile.InraRatioForageTable_EN)]
    public void ImportInraTable_return_not_null_dto_for_forage(AvailableFile forageFile)
    {
        //Arrange
        var file = FileGetter.GetFile(forageFile);

        //Act
        var res = InraRatioTableImporter.ImportInraTable(file);

        //Assert
        Assert.IsNotNull(res);
    }
    
    [Test]
    [TestCase(AvailableFile.InraRatioForageTable_FR, 913)]
    [TestCase(AvailableFile.InraRatioForageTable_EN, 913)]
    [TestCase(AvailableFile.InraRatioConcentrateTable_FR, 172)]
    [TestCase(AvailableFile.InraRatioConcentrateTable_EN, 172)]
    public void ImportInraTable_return_expected_label(AvailableFile forageFile, int expectedCount)
    {
        //Arrange
        var file = FileGetter.GetFile(forageFile);

        //Act
        var res = InraRatioTableImporter.ImportInraTable(file);

        //Assert
        var allLines = res.GetAllLines();
        Assert.AreEqual(expectedCount, allLines.Count, $"COUNT:; {allLines.Count} {Environment.NewLine}{string.Join(Environment.NewLine, allLines.Select(o => o.JoinedLabel))}");
    }

    [Test]
    [TestCase(78, @"Graines de légumineuses et d'oléagineux | Graine de coton extrudée")]
    public void ImportInraTable_return_expected_label_when_requested_by_position(int position, string value)
    {
        //Arrange
        var file = FileGetter.GetFile(AvailableFile.InraRatioConcentrateTable_FR);

        //Act
        var res = InraRatioTableImporter.ImportInraTable(file);

        //Assert
        Assert.AreEqual(value, res.GetAllLines()[position].JoinedLabel);
    }


}