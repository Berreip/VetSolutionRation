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

}