using System.Globalization;
using NUnit.Framework;
using VetSolutionRation.wpf.Views.Popups.Adapters;
using VetSolutionRationLib.Enums;
// ReSharper disable UseObjectOrCollectionInitializer

namespace VetSolutionRation.wpf.UnitTests.Adapters;

[TestFixture]
internal sealed class FeedDetailInEditionAdapterTests
{
    [Test]
    public void FeedDetailInEditionAdapter_ctor_setup_correct_initial_values()
    {
        //Arrange
        var counter = 0;
        
        //Act
        var res =new FeedDetailInEditionAdapter(InraHeader.Amidon, 56, () => Interlocked.Increment(ref counter));

        //Assert
        Assert.AreEqual(InraHeader.Amidon, res.Header);
        Assert.AreEqual("56", res.CellValue);
        Assert.AreEqual(56, res.DoubleCellValue);
        Assert.AreEqual(true, res.IsValid);
        Assert.AreEqual(0, counter);
        
    }

    [Test]
    public void Set_null_cell_value_lead_to_invalid_adapter()
    {
        //Arrange
        var counter = 0;
        var res = new FeedDetailInEditionAdapter(InraHeader.Amidon, 56, () => Interlocked.Increment(ref counter));
        
        //Act
        res.CellValue = null;

        //Assert
        Assert.AreEqual(InraHeader.Amidon, res.Header);
        Assert.AreEqual(null, res.CellValue);
        Assert.AreEqual(0, res.DoubleCellValue);
        Assert.AreEqual(false, res.IsValid);
        Assert.AreEqual(1, counter);
    }
    
    [Test]
    public void Set_invalid_cell_value_lead_to_invalid_adapter()
    {
        //Arrange
        var counter = 0;
        var res = new FeedDetailInEditionAdapter(InraHeader.Amidon, 56, () => Interlocked.Increment(ref counter));
        
        //Act
        res.CellValue = "foo";

        //Assert
        Assert.AreEqual(InraHeader.Amidon, res.Header);
        Assert.AreEqual("foo", res.CellValue);
        Assert.AreEqual(0, res.DoubleCellValue);
        Assert.AreEqual(false, res.IsValid);
        Assert.AreEqual(1, counter);
    }
    
    [Test]
    public void Set_valid_cell_value_lead_to_updated_valid_adapter()
    {
        //Arrange
        var counter = 0;
        var res = new FeedDetailInEditionAdapter(InraHeader.Amidon, 56, () => Interlocked.Increment(ref counter));
        
        //Act
        var strValue = (78.7d).ToString(CultureInfo.CurrentCulture);
        res.CellValue = strValue;

        //Assert
        Assert.AreEqual(InraHeader.Amidon, res.Header);
        Assert.AreEqual(strValue, res.CellValue);
        Assert.AreEqual(78.7d, res.DoubleCellValue, 0.0001);
        Assert.AreEqual(true, res.IsValid);
        Assert.AreEqual(0, counter);
    }
}