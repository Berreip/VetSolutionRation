using VetSolutionRationLib.Enums;

namespace VetSolutionRationLib.Models.Feed;


/// <summary>
/// A details about a nutrional field (like LysD | 2.509)
/// </summary>
public interface INutritionalFeedDetails : IFeedDetails
{
    /// <summary>
    /// The double value of this field
    /// </summary>
    double Value { get; }
}

public class NutritionalFeedDetails : INutritionalFeedDetails
{
    public NutritionalFeedDetails(InraHeader header, double value)
    {
        Header = header;
        Value = value;
    }

    /// <inheritdoc />
    public InraHeader Header { get; }

    /// <inheritdoc />
    public double Value { get; }
}