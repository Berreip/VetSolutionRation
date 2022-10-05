using VSR.Enums;

namespace VetSolutionRation.DataProvider.Models.Helpers;

public interface IStringDetailsContent 
{
    /// <summary>
    /// The details category
    /// </summary>
    InraHeader Header { get; }
    
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