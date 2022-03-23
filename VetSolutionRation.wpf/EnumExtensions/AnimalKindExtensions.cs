using System;
using VetSolutionRationLib.Enums;

namespace VetSolutionRation.wpf.EnumExtensions
{
    internal static class AnimalKindExtensions
    {
        public static string GetDisplayName(this AnimalKind kind)
        {
            switch (kind)
            {
                case AnimalKind.BovineFemale:
                    return Properties.VetSolutionRatioRes.AnimalKind_BovineFemale;
                case AnimalKind.BovineMale:
                    return Properties.VetSolutionRatioRes.AnimalKind_BovineMale;
                case AnimalKind.SheepFemale:
                    return Properties.VetSolutionRatioRes.AnimalKind_SheepFemale;
                case AnimalKind.SheepMale:
                    return Properties.VetSolutionRatioRes.AnimalKind_SheepMale;
                case AnimalKind.GoatMale:
                    return Properties.VetSolutionRatioRes.AnimalKind_GoatMale;
                case AnimalKind.GoatFemale:
                    return Properties.VetSolutionRatioRes.AnimalKind_GoatFemale;
                case AnimalKind.HorseMale:
                    return Properties.VetSolutionRatioRes.AnimalKind_HorseMale;
                case AnimalKind.HorseFemale:
                    return Properties.VetSolutionRatioRes.AnimalKind_HorseFemale;
                default:
                    throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
            }
        }
    }
}