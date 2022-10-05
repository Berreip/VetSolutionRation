namespace VSR.WPF.Utils.Helpers;

public static class SearchFilters
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
public interface ISearcheable
{
    bool MatchSearch(string[] searchText);
}