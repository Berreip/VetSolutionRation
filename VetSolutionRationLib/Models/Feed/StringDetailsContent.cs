using VetSolutionRationLib.Enums;

namespace VetSolutionRationLib.Models.Feed;

public interface IStringDetailsContent : IFeedDetails
{
    string Details { get; }
}

public class StringDetailsContent : IStringDetailsContent
{
    public StringDetailsContent(InraHeader header, string content)
    {
        Header = header;
        Details = content;
    }

    /// <inheritdoc />
    public InraHeader Header { get; }

    /// <inheritdoc />
    public string Details { get; }
}