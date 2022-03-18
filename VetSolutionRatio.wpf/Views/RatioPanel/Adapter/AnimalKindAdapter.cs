using System.Collections.Generic;
using System.Linq;
using PRF.Utils.CoreComponents.Extensions;
using PRF.WPFCore;
using VetSolutionRatio.wpf.EnumExtensions;
using VetSolutionRatioLib.Enums;
using VetSolutionRatioLib.Helpers;

namespace VetSolutionRatio.wpf.Views.RatioPanel.Adapter
{
    internal sealed class AnimalKindAdapter : ViewModelBase
    {
        private readonly AnimalKind _animalKind;
        private readonly AnimalSubKind _animalSubKind;
        private readonly string _specificDetails;
        private readonly string[] _searchPart;

        public AnimalKindAdapter(AnimalKind animalKind, AnimalSubKind animalSubKind, string specificDetails)
        {
            _animalKind = animalKind;
            _animalSubKind = animalSubKind;
            _specificDetails = specificDetails;
            _searchPart = SearchHelpers.SplitByWhitspace(new string(animalKind.GetDisplayName() + " " + animalSubKind.GetDisplayName() + " " + specificDetails));
        }

        public bool ContainsAll(string[] searchText)
        {
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

        private class SearchText
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