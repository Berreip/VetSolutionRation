using System;
using System.Globalization;
using PRF.WPFCore;
using VSR.Core.Extensions;
using VSR.Enums;
using VSR.Models.Ingredients;

namespace VSR.WPF.Utils.Adapters.EditionIngredients;

public sealed class NutritionalDetailsAdapter : ViewModelBase
{
    public InraHeader Header { get; }
    private string? _cellValue;
    private double _doubleCellValue;
    private readonly Action _refreshValidityCallBack;
    private bool _isValid = true;
    
    /// <summary>
    /// The unit of this feed (like percentage, UEM/kg, ...)
    /// </summary>
    public IFeedUnitAdapter Unit { get; }
    
    public string HeaderName { get; }
    public double DoubleCellValue => _doubleCellValue;

    public NutritionalDetailsAdapter(InraHeader header, double initialCellValue, Action refreshValidityCallBack)
    {
        Header = header;
        HeaderName = header.GetInraHeaderLabel();
        _doubleCellValue = initialCellValue;
        _refreshValidityCallBack = refreshValidityCallBack;
        _cellValue = initialCellValue.ToString(CultureInfo.CurrentCulture);
        Unit = FeedUnitAdapterFactory.CreateUnit(header.GetUnit());
    }
    
    /// <summary>
    /// Create a NutritionalDetails model from a the adapter
    /// </summary>
    public INutritionalDetails CreateNutritionalFeedDetails()
    {
        return new NutritionalDetails(Header, DoubleCellValue);
    }
    
    public string? CellValue
    {
        get => _cellValue;
        set
        {
            if (SetProperty(ref _cellValue, value))
            {
                IsValid = Unit.IsValid(value, out _doubleCellValue);
            }
        }
    }

    public bool IsValid
    {
        get => _isValid;
        private set
        {
            if (SetProperty(ref _isValid, value))
            {
                _refreshValidityCallBack.Invoke();
            }
        }
    }

    public string DetailledInformations => Header.GetDetailledInfo();
}