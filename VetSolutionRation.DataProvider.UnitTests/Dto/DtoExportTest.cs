using System;
using System.Linq;
using NUnit.Framework;
using VetSolutionRation.DataProvider.Dto;
using VetSolutionRation.DataProvider.Models;

namespace VetSolutionRation.DataProvider.UnitTests.Dto;

[TestFixture]
internal sealed class DtoExportTest
{
    [Test]
    public void ExportDto_then_import_returns_expected_data()
    {
        //Arrange
        var sut = new InraRationTableImportModel(new[]
        {
            new InraRationLineImportModel(Array.Empty<FeedCellModel>(), new[] { "foo1" }),
            new InraRationLineImportModel(Array.Empty<FeedCellModel>(), new[] { "foo2" }),
            new InraRationLineImportModel(Array.Empty<FeedCellModel>(), new[] { "foo3" }),
            new InraRationLineImportModel(Array.Empty<FeedCellModel>(), new[] { "foo4" }),
            new InraRationLineImportModel(Array.Empty<FeedCellModel>(), new[] { "foo5" }),
            new InraRationLineImportModel(Array.Empty<FeedCellModel>(), new[] { "foo6" }),
        });

        //Act
        var res = sut.ExportToDto().ImportFromDto();

        //Assert
        Assert.IsNotNull(res);
        Assert.AreEqual(6, res.GetAllLines().Count);
    }

    [Test]
    public void ExportDto_then_serialize_then_deserialize_then_import_returns_expected_data()
    {
        //Arrange
        var sut = new InraRationTableImportModel(new[]
        {
            new InraRationLineImportModel(new[]
            {
                new FeedCellModel(InraHeader.Amidon, "1"),
                new FeedCellModel(InraHeader.C12_0, "2"),
                new FeedCellModel(InraHeader.C18_0, "3"),
            }, new[] { "foo1" }),
            new InraRationLineImportModel(Array.Empty<FeedCellModel>(), new[] { "foo2" }),
        });

        //Act
        var res = DtoExporter.DeserializeFromJson(sut.ExportToDto().SerializeReferenceToJson()).ImportFromDto();

        //Assert
        Assert.IsNotNull(res);
        var lines = res.GetAllLines().ToArray();
        Assert.AreEqual(2, lines.Length);
        var cells = lines[0].GetAllCells();
        Assert.AreEqual(3, cells.Count);
        Assert.AreEqual("1", cells[InraHeader.Amidon].Content);
        Assert.AreEqual("2", cells[InraHeader.C12_0].Content);
        Assert.AreEqual("3", cells[InraHeader.C18_0].Content);
    }
}