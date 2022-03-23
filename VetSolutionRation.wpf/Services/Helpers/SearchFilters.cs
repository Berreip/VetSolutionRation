using VetSolutionRation.wpf.Views.RatioPanel.Adapter;

namespace VetSolutionRation.wpf.Services.Helpers;

internal static class SearchFilters
{
    public static bool FilterAnimalKind(object obj, string[] searchText)
    {
        if (searchText.Length == 0)
        {
            // if no filter, show all.
            return true;
        }
        if (obj is IAnimalAdapter animal)
        {
            return animal.ContainsAll(searchText);
        }
        return false;
    }
}