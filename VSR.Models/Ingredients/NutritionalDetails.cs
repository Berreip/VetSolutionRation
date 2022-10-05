using VSR.Enums;

namespace VSR.Models.Ingredients;


/// <summary>
/// A details about a nutrional field (like LysD | 2.509)
/// </summary>
public interface INutritionalDetails
{
    /// <summary>
    /// The double value of this field
    /// </summary>
    double Value { get; }
    
    InraHeader Header { get; }
}

public class NutritionalDetails : INutritionalDetails
{
    public NutritionalDetails(InraHeader header, double value)
    {
        Header = header;
        Value = value;
    }

    /// <inheritdoc />
    public InraHeader Header { get; }

    /// <inheritdoc />
    public double Value { get; }
}