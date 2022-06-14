using System;
using VetSolutionRationLib.Enums;

namespace VetSolutionRation.wpf.Helpers;

internal static class FeedUnitWpfExtensions
{
    public static string ToDiplayName(this FeedUnit unit)
    {
        switch (unit)
        {
            case FeedUnit.Kg:
                return "kg";
            default:
                throw new ArgumentOutOfRangeException(nameof(unit), unit, null);
        }
        
    }

    /// <summary>
    /// Convert from unit to kg
    /// </summary>
    public static double GetNormalizedQuantityInKg(this FeedUnit unit, double quantity)
    {
        switch (unit)
        {
            case FeedUnit.Kg:
                return quantity;
            default:
                throw new ArgumentOutOfRangeException(nameof(unit), unit, null);
        }
        
    }
}