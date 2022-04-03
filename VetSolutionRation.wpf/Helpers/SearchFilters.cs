namespace VetSolutionRation.wpf.Helpers;

internal static class SearchFilters
{
    public static bool FilterParts(object item, string[] searchText)
    {
        if (searchText.Length == 0)
        {
            // if no filter, show all.
            return true;
        }

        if (item is ISearcheable searcheable)
        {
            return searcheable.MatchSearch(searchText);
        }

        return false;
    }
}

/// <summary>
/// define a searcheable item
/// </summary>
internal interface ISearcheable
{
    bool MatchSearch(string[] searchText);
}