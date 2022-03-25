using System.Collections.Generic;
using System.Linq;
using PRF.Utils.CoreComponents.Extensions;
using PRF.WPFCore;
using VetSolutionRation.wpf.Helpers;
using VetSolutionRationLib.Helpers;

namespace VetSolutionRation.wpf.Searcheable;

internal abstract class SearcheableBase : ViewModelBase, ISearcheable
{
    private readonly string[] _searchPart;

    protected SearcheableBase(string searchPart)
    {
        _searchPart = SearchHelpers.SplitByWhitspace(searchPart);
    }

    /// <inheritdoc />
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