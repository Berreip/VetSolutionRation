using System.Linq;
using VetSolutionRation.wpf.EnumExtensions;
using VetSolutionRation.wpf.Searcheable;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Helpers;

namespace VetSolutionRation.wpf.Views.RatioPanel.Adapter
{
    internal sealed class AnimalAdapter : SearcheableBase
    {
        private readonly AnimalKind _animalKind;
        private readonly AnimalSubKind _animalSubKind;
        private readonly string _specificDetails;

        public AnimalAdapter(AnimalKind animalKind, AnimalSubKind animalSubKind, string specificDetails) 
            : base(animalKind.GetDisplayName() + " " + animalSubKind.GetDisplayName() + " " + specificDetails)
        {
            _animalKind = animalKind;
            _animalSubKind = animalSubKind;
            _specificDetails = specificDetails;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var parts = new[]
            {
                _animalKind.GetDisplayName(),
                _animalSubKind.GetDisplayName(),
                _specificDetails
            };
            return string.Join(" | ", parts.Where(s => !string.IsNullOrWhiteSpace(s)));
        }
    }
}