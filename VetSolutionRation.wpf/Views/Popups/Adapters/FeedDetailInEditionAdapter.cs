using System;
using System.Globalization;
using PRF.WPFCore;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models.Feed;

namespace VetSolutionRation.wpf.Views.Popups.Adapters;

internal sealed class FeedDetailInEditionAdapter : ViewModelBase
{
    public InraHeader Header { get; }
    private string? _cellValue;
    private double _doubleCellValue;
    private readonly Action _refreshValidityCallBack;
    private bool _isValid = true;

    public string HeaderName { get; }
    public double DoubleCellValue => _doubleCellValue;

    public FeedDetailInEditionAdapter(InraHeader header, double initialCellValue, Action refreshValidityCallBack)
    {
        Header = header;
        HeaderName = header.GetInraHeaderLabel();
        _doubleCellValue = initialCellValue;
        _refreshValidityCallBack = refreshValidityCallBack;
        _cellValue = initialCellValue.ToString(CultureInfo.CurrentCulture);
    }
    
    public string? CellValue
    {
        get => _cellValue;
        set
        {
            if (SetProperty(ref _cellValue, value))
            {
                IsValid = double.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out _doubleCellValue);
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

internal static class FeedDetailInEditionAdapterExtensions
{
    /// <summary>
    /// Create a NutritionalFeedDetails model from a the adapter
    /// </summary>
    public static INutritionalFeedDetails CreateNutritionalFeedDetails(this FeedDetailInEditionAdapter adapter)
    {
        return new NutritionalFeedDetails(adapter.Header, adapter.DoubleCellValue);
    }
}