using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using PRF.Utils.CoreComponents.JSON;
using VetSolutionRation.DataProvider.Dto;
using VetSolutionRation.DataProvider.Models;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models.Feed;

namespace VetSolutionRation.DataProvider.UnitTests.Dto;

[TestFixture]
internal sealed class DtoExportTest
{
    [Test]
    public void ExportDto_then_import_returns_expected_data()
    {
        //Arrange
        var sut = new IFeed[]
        {
            new ReferenceFeed(new[] { "foo1" }, Array.Empty<INutritionalFeedDetails>(), Array.Empty<IStringDetailsContent>()),
            new ReferenceFeed(new[] { "foo2" }, Array.Empty<INutritionalFeedDetails>(), Array.Empty<IStringDetailsContent>()),
            new ReferenceFeed(new[] { "foo3" }, Array.Empty<INutritionalFeedDetails>(), Array.Empty<IStringDetailsContent>()),
            new ReferenceFeed(new[] { "foo4" }, Array.Empty<INutritionalFeedDetails>(), Array.Empty<IStringDetailsContent>()),
            new ReferenceFeed(new[] { "foo5" }, Array.Empty<INutritionalFeedDetails>(), Array.Empty<IStringDetailsContent>()),
            new ReferenceFeed(new[] { "foo6" }, Array.Empty<INutritionalFeedDetails>(), Array.Empty<IStringDetailsContent>()),
        };

        //Act
        var res = DtoExporter.DeserializeFromJson(sut.ConvertToDto().SerializeToJson());

        //Assert
        Assert.IsNotNull(res);
        Assert.AreEqual(6, res.Feeds?.Count);
    }

    [Test]
    public void ExportDto_then_serialize_then_deserialize_then_import_returns_expected_data()
    {
        //Arrange
        var sut = new IFeed[]
        {
            new ReferenceFeed(new[] { "foo1" }, 
                new INutritionalFeedDetails[]
                {
                    new NutritionalFeedDetails(InraHeader.Amidon, 1),
                    new NutritionalFeedDetails(InraHeader.C12_0, 2),
                    new NutritionalFeedDetails(InraHeader.C18_0, 3),
                }, 
                new IStringDetailsContent[]
                {
                    new StringDetailsContent(InraHeader.Amidon, "foo"),
                }),
        };

        //Act
        var res = DtoExporter.DeserializeFromJson(sut.ConvertToDto().SerializeToJson());

        //Assert
        Assert.IsNotNull(res);
        if (res.Feeds == null)
        {
            throw new ArgumentException("res.Feeds is null");
        }
        var lines = res.Feeds.ToArray();
        Assert.AreEqual(1, lines.Length);
        
        var cells = lines[0].NutritionDetails;
        if (cells == null)
        {
            throw new ArgumentException("cells is null");
        }
        Assert.AreEqual(3, cells.Count);
        Assert.AreEqual(1, cells[0].CellContent);
        Assert.AreEqual(2, cells[1].CellContent);
        Assert.AreEqual(3, cells[2].CellContent);
        
        var stringDetails = lines[0].StringDetails;
        if (stringDetails == null)
        {
            throw new ArgumentException("stringDetails is null");
        }
        Assert.AreEqual(1, stringDetails.Count);
        Assert.AreEqual("foo", stringDetails[0].CellContent);
    }
}