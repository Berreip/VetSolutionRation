using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PRF.WPFCore;
using VetSolutionRation.wpf.Helpers;
using VetSolutionRationLib.Helpers;

namespace VetSolutionRation.wpf.Searcheable;

internal abstract class SearcheableBase : ViewModelBase, ISearcheable
{
    private readonly string[] _searchPart;

    protected SearcheableBase(string searchPart)
    {
        _searchPart = SearchHelpers.SplitByWhitspaceAndSpecificSymbols(searchPart);
    }

    /// <inheritdoc />
    public bool MatchSearch(string[] searchText)
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
            if (!part.AlreadyFound)
            {
                if (part.Found(searchPart))
                {
                    part.AlreadyFound = true;
                    return true;
                }
            }
        }

        return false;
    }

    private sealed class SearchText
    {
        private readonly string _text;
        public bool AlreadyFound { get; set; }

        public SearchText(string text)
        {
            _text = text;
        }

        public bool Found(string searchPart)
        {
            // do a start with equivalent with accent and case ignored
            return CultureInfo.InvariantCulture.CompareInfo.IsPrefix(
                _text, 
                searchPart, 
                CompareOptions.IgnoreCase | CompareOptions.IgnoreSymbols | CompareOptions.IgnoreNonSpace);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"[{_text}] AlreadyFound ={AlreadyFound}";
        }
    }
}