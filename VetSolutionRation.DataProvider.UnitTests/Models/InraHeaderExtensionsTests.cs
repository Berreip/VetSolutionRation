using System;
using System.Collections.Generic;
using NUnit.Framework;
using VetSolutionRation.DataProvider.Models;

namespace VetSolutionRation.DataProvider.UnitTests.Models;

[TestFixture]
internal sealed class InraHeaderExtensionsTests
{
    [Test]
    [TestCase(InraSourceFileCulture.French)]
    [TestCase(InraSourceFileCulture.English)]
    public void GetInraHeaderLabel_returns_label_for_all_enum_value(InraSourceFileCulture culture)
    {
        //Arrange

        //Act
        foreach (InraHeader inraHeader in Enum.GetValues(typeof(InraHeader)))
        {
            var res = inraHeader.GetInraHeaderLabel(culture);
            Assert.IsNotNull(res);
        }

        //Assert
    }

    [Test]
    [TestCase(InraSourceFileCulture.French)]
    [TestCase(InraSourceFileCulture.English)]
    public void TryParseInraHeader_returns_label_for_all_enum_value(InraSourceFileCulture culture)
    {
        //Arrange

        //Act
        foreach (InraHeader inraHeader in Enum.GetValues(typeof(InraHeader)))
        {
            var label = inraHeader.GetInraHeaderLabel(culture);
            var parseSuccess = InraHeaderExtensions.TryParseInraHeader(label, culture, out var parsedHeader);
            Assert.IsTrue(parseSuccess, $"failed to parse {label}");
            Assert.AreEqual(inraHeader, parsedHeader);
        }

        //Assert
    }

    [Test]
    [TestCase(InraSourceFileCulture.French)]
    [TestCase(InraSourceFileCulture.English)]
    public void TryParseInraHeader_returns_No_duplication_label_for_all_enum_value(InraSourceFileCulture culture)
    {
        //Arrange
        var duplicateHash = new Dictionary<InraHeader, string>();

        //Act
        foreach (InraHeader inraHeader in Enum.GetValues(typeof(InraHeader)))
        {
            var label = inraHeader.GetInraHeaderLabel(culture);
            var parseSuccess = InraHeaderExtensions.TryParseInraHeader(label, culture, out var parsedHeader);
            Assert.IsTrue(parseSuccess);
            Assert.IsFalse(duplicateHash.TryGetValue(parsedHeader, out var duplicated), $"the label {label} lead to a duplicate key: {parsedHeader} also used by {duplicated}");
            duplicateHash.Add(parsedHeader, label);
        }

        //Assert
    }

    [Test]
    public void GetInraHeaderLabel_throw_for_undefined_value()
    {
        //Arrange

        //Act
        Assert.Throws<ArgumentOutOfRangeException>(() => ((InraHeader)(-1)).GetInraHeaderLabel(InraSourceFileCulture.English));

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
        var parseSuccess = InraHeaderExtensions.TryParseInraHeader(str, InraSourceFileCulture.French, out var parsedHeader);

        //Assert
        Assert.IsTrue(parseSuccess);
        Assert.AreEqual(expectedInraHeader, parsedHeader);
    }
    
    [Test]
    public void TryParseDtoInraHeader_returns_expected_values_for_all_enums_members()
    {
        //Arrange

        //Act
        foreach (InraHeader inraHeader in Enum.GetValues(typeof(InraHeader)))
        {
            var label = inraHeader.ToDtoKey();
            Assert.IsNotEmpty(label);
            var parseSuccess = InraHeaderExtensions.TryParseDtoInraHeader(label, out var parsedHeader);
            Assert.IsTrue(parseSuccess, $"failed to parse {label}");
            Assert.AreEqual(inraHeader, parsedHeader);
        }

        //Assert
    }
}