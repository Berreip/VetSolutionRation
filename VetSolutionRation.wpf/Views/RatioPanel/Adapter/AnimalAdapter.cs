using System.Collections.Generic;
using System.Linq;
using PRF.Utils.CoreComponents.Extensions;
using PRF.WPFCore;
using VetSolutionRation.wpf.EnumExtensions;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Helpers;

namespace VetSolutionRation.wpf.Views.RatioPanel.Adapter
{
    internal interface IAnimalAdapter
    {
        bool ContainsAll(string[] searchText);
    }

    internal sealed class AnimalAdapter : ViewModelBase, IAnimalAdapter
    {
        private readonly AnimalKind _animalKind;
        private readonly AnimalSubKind _animalSubKind;
        private readonly string _specificDetails;
        private readonly string[] _searchPart;

        public AnimalAdapter(AnimalKind animalKind, AnimalSubKind animalSubKind, string specificDetails)
        {
            _animalKind = animalKind;
            _animalSubKind = animalSubKind;
            _specificDetails = specificDetails;
            _searchPart = SearchHelpers.SplitByWhitspace(animalKind.GetDisplayName() + " " + animalSubKind.GetDisplayName() + " " + specificDetails);
        }

        public bool ContainsAll(string[] searchText)
        {
            // we create a search text object that will hold the 'isfound' during searching
            var parts = _searchPart.Select(o => new SearchText(o)).ToArray();
            foreach (var searchPart in searchText)
            {
                if (!PartFound(parts, searchPart))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool PartFound(IReadOnlyCollection<SearchText> parts, string searchPart)
        {
            foreach (var part in parts)
            {
                if (part.AlreadyFound == false)
                {
                    if (part.ContainsInsensitive(searchPart))
                    {
                        part.AlreadyFound = true;
                        return true;
                    }
                }
            }

            return false;
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

        private sealed class SearchText
        {
            private readonly string _text;
            public bool AlreadyFound { get; set; }

            public SearchText(string text)
            {
                _text = text;
            }

            public bool ContainsInsensitive(string searchPart)
            {
                return _text.ContainsInsensitive(searchPart);
            }

            /// <inheritdoc />
            public override string ToString()
            {
                return $"[{_text}] AlreadyFound ={AlreadyFound}";
            }
        }
    }
}