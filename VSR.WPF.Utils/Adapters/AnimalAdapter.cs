﻿using VSR.Enums;
using VSR.WPF.Utils.Adapters.Searcheable;

namespace VSR.WPF.Utils.Adapters
{
    public sealed class AnimalAdapter : SearcheableBase
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