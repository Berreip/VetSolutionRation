using NUnit.Framework;
using VetSolutionRatio.DataProvider.UnitTests.Data;

namespace VetSolutionRatio.DataProvider.UnitTests;

[TestFixture]
internal sealed class Tests
{
    [Test]
    public void ImportInraTable_return_not_null_dto_for_concentrate()
    {
        //Arrange
        var file = FileGetter.GetFile(AvailableFile.InraRatioConcentrateTable);

        //Act
        var res = InraRatioTableImporter.ImportInraTable(file);

        //Assert
        Assert.IsNotNull(res);
    }

    [Test]
    public void ImportInraTable_return_not_null_dto_for_forage()
    {
        //Arrange
        var file = FileGetter.GetFile(AvailableFile.InraRatioForageTable);

        //Act
        var res = InraRatioTableImporter.ImportInraTable(file);

        //Assert
        Assert.IsNotNull(res);
    }

}