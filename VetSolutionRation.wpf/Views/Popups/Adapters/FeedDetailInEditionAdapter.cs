using PRF.WPFCore;
using VetSolutionRationLib.Enums;

namespace VetSolutionRation.wpf.Views.Popups.Adapters;

internal sealed class FeedDetailInEditionAdapter : ViewModelBase
{
    public InraHeader Header { get; }
    private string? _cellValue;

    public FeedDetailInEditionAdapter(InraHeader header)
    {
        Header = header;
        HeaderName = header.GetInraHeaderLabel();
        _cellValue = "0";
    }

    public string HeaderName { get; }

    public string? CellValue
    {
        get => _cellValue;
        set => SetProperty(ref _cellValue, value);
    }
}