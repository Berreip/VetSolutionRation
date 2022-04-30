using NUnit.Framework;
using VetSolutionRation.DataProvider.Services.Excel.ExcelUtils;

namespace VetSolutionRation.DataProvider.UnitTests.Excel;

[TestFixture]
internal sealed class ExcelHelpersTests
{
    [Test]
    [TestCase("A", 0)]
    [TestCase("A1", 0)]
    [TestCase("A2", 0)]
    [TestCase("a2", 0)]
    [TestCase("A2780", 0)]
    [TestCase("B1", 1)]
    [TestCase("B2", 1)]
    [TestCase("C1", 2)]
    [TestCase("D1", 3)]
    [TestCase("E1", 4)]
    [TestCase("F1", 5)]
    [TestCase("G1", 6)]
    [TestCase("H1", 7)]
    [TestCase("I1", 8)]
    [TestCase("J1", 9)]
    [TestCase("K1", 10)]
    [TestCase("L1", 11)]
    [TestCase("M1", 12)]
    [TestCase("N1", 13)]
    [TestCase("O1", 14)]
    [TestCase("P1", 15)]
    [TestCase("Q1", 16)]
    [TestCase("R1", 17)]
    [TestCase("S1", 18)]
    [TestCase("T1", 19)]
    [TestCase("U1", 20)]
    [TestCase("V1", 21)]
    [TestCase("W1", 22)]
    [TestCase("X1", 23)]
    [TestCase("Y1", 24)]
    [TestCase("Z1", 25)]
    [TestCase("AA1", 26)]
    [TestCase("AA", 26)]
    [TestCase("AB", 27)]
    [TestCase("AC", 28)]
    [TestCase("AAB", 703)]
    [TestCase("CC", 80)]
    [TestCase("BA2", 52)]
    [TestCase("BC2", 54)]
    public void GetColumnPositionFromCellReference_returns_expected_position(string cellReference, int expectedPosition)
    {
        //Arrange

        //Act
        var res = ExcelHelpers.GetColumnPositionFromCellReference(cellReference);

        //Assert
        Assert.AreEqual(expectedPosition, res);
    }

}