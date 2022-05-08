using PRF.WPFCore;
using VetSolutionRationLib.Enums;

namespace VetSolutionRation.wpf.Views.Popups.Adapters;

internal sealed class HeaderAdapter : ViewModelBase
{
    public string Header { get; }
    public InraHeader HeaderKind { get; }

    public HeaderAdapter(InraHeader header)
    {
        HeaderKind = header;
        Header = header.GetInraHeaderLabel();
    }

    /// <inheritdoc />
    public override string ToString() => Header;
}