using System;
using VetSolutionRatioLib.Enums;

namespace VetSolutionRatio.wpf.EnumExtensions
{
    internal static class AnimalKindExtensions
    {
        public static string GetDisplayName(this AnimalKind kind)
        {
            switch (kind)
            {
                case AnimalKind.Cow:
                    return Properties.VetSolutionRatioRes.AnimalKind_Cow;
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }
    }
}