using System;
using System.Globalization;
using PRF.WPFCore;
using VetSolutionRationLib.Models.Units;

namespace VetSolutionRation.wpf.Views.Popups.Adapters;

public interface IFeedUnitAdapter
{
    bool IsValid(string? cellValue, out double doubleCellValue);
    public string DisplayLabel { get; }
}

public static class FeedUnitAdapterFactory
{
    /// <summary>
    /// Create the unit adapter according to the unit
    /// </summary>
    public static IFeedUnitAdapter CreateUnit(IUnit unit)
    {
        switch (unit)
        {
            case IPercentageUnit percentageUnit:
                return new PercentageUnitFeedAdapter(percentageUnit);
            case ISpecificUnit specificUnit:
                return new SpecificUnitFeedAdapter(specificUnit);
            default:
                return new NoUnitFeedAdapter();
        }
    }
}


public sealed class PercentageUnitFeedAdapter : ViewModelBase, IFeedUnitAdapter
{
    private readonly IPercentageUnit _unit;

    public PercentageUnitFeedAdapter(IPercentageUnit unit)
    {
        _unit = unit;  
        DisplayLabel = unit.DisplayMention;
    }

    /// <inheritdoc />
    public override string ToString() => _unit.DisplayMention;
    
    /// <inheritdoc />
    public bool IsValid(string? cellValue, out double doubleCellValue)
    {
        if (double.TryParse(cellValue, NumberStyles.Any, CultureInfo.CurrentCulture, out doubleCellValue)
            && doubleCellValue >= 0 && doubleCellValue <= 100)
        {
            return true;
        }
        return false;
    }

    /// <inheritdoc />
    public string DisplayLabel { get; }
}

public sealed class SpecificUnitFeedAdapter : ViewModelBase, IFeedUnitAdapter
{
    private readonly ISpecificUnit _unit;

    public SpecificUnitFeedAdapter(ISpecificUnit unit)
    {
        _unit = unit;
        DisplayLabel = unit.DisplayMention;
    }

    /// <inheritdoc />
    public override string ToString() => _unit.DisplayMention;
    
    /// <inheritdoc />
    public bool IsValid(string? cellValue, out double doubleCellValue)
    {
        return double.TryParse(cellValue, NumberStyles.Any, CultureInfo.CurrentCulture, out doubleCellValue);
    }

    /// <inheritdoc />
    public string DisplayLabel { get; }
}

public sealed class NoUnitFeedAdapter : ViewModelBase, IFeedUnitAdapter
{
    /// <inheritdoc />
    public bool IsValid(string? cellValue, out double doubleCellValue)
    {
        return double.TryParse(cellValue, NumberStyles.Any, CultureInfo.CurrentCulture, out doubleCellValue);
    }

    /// <inheritdoc />
    public string DisplayLabel { get; } = string.Empty;
}