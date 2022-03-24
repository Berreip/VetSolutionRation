namespace VetSolutionRation.wpf.Services.Helpers;

internal static class SearchFilters
{
    public static bool FilterParts(object item, string[] searchText)
    {
        if (searchText.Length == 0)
        {
            // if no filter, show all.
            return true;
        }

        if (item is ISearcheable animal)
        {
            return animal.ContainsAll(searchText);
        }

        return false;
    }
}

/// <summary>
/// define a searcheable item
/// </summary>
internal interface ISearcheable
{
    bool ContainsAll(string[] searchText);
}