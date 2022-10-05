using System.Collections.Generic;
using System.Linq;

namespace VSR.Enums;

public enum FeedUnit
{
    Kg,
}

public static class FeedUnitExtensions 
{
    private static readonly Dictionary<string, FeedUnit> _referenceToUnit;
    private static readonly Dictionary<FeedUnit,string> _unitToReference;

    static FeedUnitExtensions()
    {
        _referenceToUnit = new Dictionary<string, FeedUnit>
        {
            {"KG", FeedUnit.Kg},
        };
        _unitToReference = _referenceToUnit.ToDictionary(o => o.Value, o => o.Key);
    }
    
    public static string ToReferenceLabel(this FeedUnit unit)
    {
        return _unitToReference[unit];
    }

    /// <summary>
    /// Convert from unit to kg
    /// </summary>
    public static FeedUnit ParseFromReferenceLabel(string? referenceLabel)
    {
        if (referenceLabel != null && _referenceToUnit.TryGetValue(referenceLabel, out var unit))
        {
            return unit;
        }
        return FeedUnit.Kg; // default value
    }
}