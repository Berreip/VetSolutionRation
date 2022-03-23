using NUnit.Framework;
using VetSolutionRation.DataProvider.Utils;

namespace VetSolutionRation.DataProvider.UnitTests.Utils;

[TestFixture]
internal sealed class LabelRegexHolderTests
{
    [Test]
    [TestCase(@"Libellé 0", 0, true)]
    [TestCase(@"Libellé 1", 1, true)]
    [TestCase(@"Libellé 2", 2, true)]
    [TestCase(@"Libellé 3", 3, true)]
    [TestCase(@"Libellé 4", 4, true)]
    [TestCase(@"Libellé 467", 467, true)]
    [TestCase(@"Label0", 0, true)]
    [TestCase(@"Label1", 1, true)]
    [TestCase(@"Label2", 2, true)]
    [TestCase(@"Label3", 3, true)]
    [TestCase(@"Label4", 4, true)]
    [TestCase(@"Label467", 467, true)]
    [TestCase(@"gfdhdfhgyuè", -1, false)]
    [TestCase(@"", -1, false)]
    [TestCase(@"Label1 6 7", -1, false)]
    [TestCase(@"Libellé 4d", -1, false)]
    public void LabelRegexHolder_returns_expected_result(string inputStr, int expectedLabelPosition, bool expectedSucess)
    {
        //Arrange

        //Act
        var res = LabelRegexHolder.Match(inputStr, out var labelPosition);

        //Assert
        Assert.AreEqual(expectedSucess, res);
        Assert.AreEqual(expectedLabelPosition, labelPosition);
        

    }

    
}