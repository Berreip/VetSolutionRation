using System;
using VSR.WPF.Utils.Adapters.IngredientsAndRecipeList;
using VSR.WPF.Utils.Services;

namespace VSR.WPF.Utils.Helpers;

public static class SearchFilters
{
    public static bool FilterAnimals(object item, string[] searchText)
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
    
    public static bool FilterIngredientsOrRecipe(object item, string[] searchText, FilterKind lastFilterKind)
    {
        switch (lastFilterKind)
        {
            case FilterKind.Ingredient:
                if (item is not IngredientForListAdapterBase)
                {
                    return false;
                }
                break;
            case FilterKind.Recipe:
                if (item is not RecipeForListAdapter)
                {
                    return false;
                }
                break;
            // ReSharper disable once RedundantEmptySwitchSection
            default:
                // no filter by type if any other
                break;
        }
        
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