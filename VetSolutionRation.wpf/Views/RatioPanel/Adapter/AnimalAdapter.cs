using System;
using VetSolutionRation.wpf.Searcheable;
using VetSolutionRationLib.Enums;

namespace VetSolutionRation.wpf.Views.RatioPanel.Adapter
{
    internal sealed class AnimalAdapter : SearcheableBase
    {
        private bool _isSelected;
        public AnimalKind Kind { get; }
        public string DisplayName { get; }

        public AnimalAdapter(AnimalKind kind, string animaldescription) : base(animaldescription)
        {
            Kind = kind;
            DisplayName = animaldescription;
        }

        public bool IsSelected
        {
            get => _isSelected;
            set => SetProperty(ref _isSelected, value);
        }

        /// <inheritdoc />
        public override string ToString() => DisplayName;
    }
}