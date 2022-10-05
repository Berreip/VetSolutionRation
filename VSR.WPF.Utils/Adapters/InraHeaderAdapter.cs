using PRF.WPFCore;
using VSR.Core.Extensions;
using VSR.Enums;

namespace VSR.WPF.Utils.Adapters;

public sealed class InraHeaderAdapter : ViewModelBase
{
    public string Header { get; }
    public InraHeader HeaderKind { get; }

    public InraHeaderAdapter(InraHeader header)
    {
        HeaderKind = header;
        Header = header.GetInraHeaderLabel();
    }

    /// <inheritdoc />
    public override string ToString() => Header;
}