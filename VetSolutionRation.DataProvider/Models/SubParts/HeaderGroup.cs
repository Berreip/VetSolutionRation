namespace VetSolutionRation.DataProvider.Models.SubParts;

internal sealed class HeaderGroup
{
    public string GroupHeader { get; }

    public HeaderGroup(int groupStartingIndex, string groupHeader)
    {
        GroupHeader = groupHeader;
        StartingIndex = groupStartingIndex;
    }

    public int StartingIndex { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{GroupHeader} from [{StartingIndex}]";
    }
}