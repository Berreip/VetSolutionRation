namespace VetSolutionRationLib.Models.Units;

/// <summary>
/// Represent a feed unit
/// </summary>
public interface IUnit
{
    UnitKind UnitKind { get; }
}

public abstract class UnitBase : IUnit
{
    public UnitKind UnitKind { get; }
    
    protected UnitBase(UnitKind unitKind)
    {
        UnitKind = unitKind;
    }
}

public static class Units
{
    /// <summary>
    /// Percentage unit
    /// </summary>
    public static IUnit Percentage { get; } = new PercentageUnit();
    
    /// <summary>
    /// g/kg MS unit
    /// </summary>
    public static IUnit GKgMs { get; } = new SpecificUnit(@"g/kg MS");

    /// <summary>
    /// mg/kg MS unit
    /// </summary>
    public static IUnit MgKgMs { get; } = new SpecificUnit(@"mg/kg MS");

    /// <summary>
    /// No unit provided
    /// </summary>
    public static IUnit NoUnit { get; } = new NoUnit();
}

public enum UnitKind
{
    /// <summary>
    /// nothing defined
    /// </summary>
    NoUnit, 
    /// <summary>
    /// a percentage
    /// </summary>
    Percentage,
    
    /// <summary>
    /// a specific unit (like 'g/kg MS', ...)
    /// </summary>
    Specific,
}