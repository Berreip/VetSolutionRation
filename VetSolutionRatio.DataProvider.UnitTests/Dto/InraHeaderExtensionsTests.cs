using System;
using NUnit.Framework;
using VetSolutionRatio.DataProvider.Dto;

namespace VetSolutionRatio.DataProvider.UnitTests.Dto;

[TestFixture]
internal sealed class InraHeaderExtensionsTests
{
    [Test]
    public void GetInraHeaderLabel_returns_label_for_all_enum_value()
    {
        //Arrange

        //Act
        foreach (InraHeader inraHeader in Enum.GetValues(typeof(InraHeader)))
        {
            var res = inraHeader.GetInraHeaderLabel();
            Assert.IsNotNull(res);
        }

        //Assert
    }

    [Test]
    public void TryParseInraHeader_returns_label_for_all_enum_value()
    {
        //Arrange

        //Act
        foreach (InraHeader inraHeader in Enum.GetValues(typeof(InraHeader)))
        {
            var label = inraHeader.GetInraHeaderLabel();
            var parseSuccess = InraHeaderExtensions.TryParseInraHeader(label, out var parsedHeader);
            Assert.IsTrue(parseSuccess);
            Assert.AreEqual(inraHeader, parsedHeader);
        }

        //Assert
    }

    [Test]
    public void GetInraHeaderLabel_throw_for_undefined_value()
    {
        //Arrange

        //Act
        Assert.Throws<ArgumentOutOfRangeException>(() => ((InraHeader)(-1)).GetInraHeaderLabel());

        //Assert
    }

    [Test]
    [TestCase("Code INRA", InraHeader.CodeInra)]
    [TestCase("Code inra", InraHeader.CodeInra)]
    [TestCase("COde INRA", InraHeader.CodeInra)]
    public void TryParseInraHeader_is_not_case_sensitive(string str, InraHeader expectedInraHeader)
    {
        //Arrange

        //Act
        var parseSuccess = InraHeaderExtensions.TryParseInraHeader(str, out var parsedHeader);

        //Assert
        Assert.IsTrue(parseSuccess);
        Assert.AreEqual(expectedInraHeader, parsedHeader);
    }
}