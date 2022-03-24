using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using VetSolutionRation.DataProvider.Models.SubParts;
using VetSolutionRation.DataProvider.Services.Excel.ExcelDto;

namespace VetSolutionRation.DataProvider.UnitTests.Models.SubParts;

[TestFixture]
internal sealed class InraGroupCategoriesTests
{
    [Test]
    [TestCase(92, 92)]
    [TestCase(100, 92)]
    [TestCase(91, 58)]
    [TestCase(58, 58)]
    [TestCase(57, 43)]
    [TestCase(43, 43)]
    [TestCase(42, 0)]
    [TestCase(10, 0)]
    [TestCase(0, 0)]
    public void InraGroupCategories_returns_expected_values(int targetIndex, int groupRefStratingIndex)
    {
        //Arrange
        var row = new Mock<IExcelRowDto>();
        row.Setup(o => o.Cells).Returns(new Dictionary<int, string>
        {
            { 0, "Valeurs de la table imprimée" },
            { 43, "Minéraux et Vitamines (valeurs supplémentaires)" },
            { 58, "Acides aminés by-pass et Acides aminés digestibles dans l'intestin (valeurs supplémentaires)" },
            { 92, "Profils d'acides gras (valeurs supplémentaires)" },
        });
        var sut = new InraGroupCategories(row.Object);

        //Act
        var res = sut.GetGroupByIndex(targetIndex);

        //Assert
        Assert.AreEqual(res.StartingIndex, groupRefStratingIndex);
        

    }

    
}