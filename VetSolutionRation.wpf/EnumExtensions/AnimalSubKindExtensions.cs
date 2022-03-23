using System;
using VetSolutionRationLib.Enums;

namespace VetSolutionRation.wpf.EnumExtensions
{
    internal static class AnimalSubKindExtensions
    {
        public static string GetDisplayName(this AnimalSubKind kind)
        {
            switch (kind)
            {
                case AnimalSubKind.Undefined:
                    return string.Empty;
                case AnimalSubKind.Heifer:
                    return Properties.VetSolutionRatioRes.AnimalSubKind_Heifer;
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }
    }
}