using System;
using System.Collections.Generic;
using System.ComponentModel;
using PRF.Utils.CoreComponents.Diagnostic;
using PRF.WPFCore;
using PRF.WPFCore.CustomCollections;
using VSR.Core.Helpers;
using VSR.Core.Services;
using VSR.WPF.Utils.Adapters.IngredientsAndRecipeList;
using VSR.WPF.Utils.Helpers;

namespace VSR.WPF.Utils.Services;

public interface IIngredientAdaptersHoster
{
    /// <summary>
    /// the main list mixing ingredients (user defined and custom + recipes)
    /// </summary>
    ICollectionView AvailableIngredientAndRecipes { get; }

    string? SearchFilter { get; set; }
    void SetFilterKind(FilterKind kind);
}

public enum FilterKind
{
    All,
    Ingredient,
    Recipe
}

internal sealed class IngredientAdaptersHoster : ViewModelBase, IIngredientAdaptersHoster
{
    private string? _searchText;
    private readonly ObservableCollectionRanged<IIngredientOrRecipeForListAdapter> _availableFeeds;

    private readonly Dictionary<Guid, IIngredientOrRecipeForListAdapter> _adapterByGuid;
    private FilterKind _lastFilterKind = FilterKind.All;
    public ICollectionView AvailableIngredientAndRecipes { get; }

    public IngredientAdaptersHoster(IIngredientsManager ingredientsManager)
    {
        _adapterByGuid = InitializeAdapters(ingredientsManager);

        AvailableIngredientAndRecipes = ObservableCollectionSource.GetDefaultView(_adapterByGuid.Values, out _availableFeeds);
        AvailableIngredientAndRecipes.SortDescriptions.Add(new SortDescription(nameof(IIngredientOrRecipeForListAdapter.DisplayPriority), ListSortDirection.Descending));
        AvailableIngredientAndRecipes.SortDescriptions.Add(new SortDescription(nameof(IIngredientOrRecipeForListAdapter.Name), ListSortDirection.Ascending)); // then in aphabetical order

        ingredientsManager.OnIngredientsChanged += OnIngredientsChanged;
        ingredientsManager.OnRecipesChanged += OnRecipesChanged;
    }

    private static Dictionary<Guid, IIngredientOrRecipeForListAdapter> InitializeAdapters(IIngredientsManager feedProvider)
    {
        var adapters = new Dictionary<Guid, IIngredientOrRecipeForListAdapter>();
        foreach (var ingredient in feedProvider.GetAllIngredients())
        {
            adapters.Add(ingredient.Guid, ingredient.IsUserAdded ? new UserDefinedIngredientForListAdapter(ingredient) : new ReferenceIngredientForListAdapter(ingredient));
        }

        foreach (var recipe in feedProvider.GetAllRecipes())
        {
            adapters.Add(recipe.Guid, new RecipeForListAdapter(recipe));
        }

        return adapters;
    }

    private void FilterAvailableFeeds(string? inputText)
    {
        if (inputText == null && _lastFilterKind == FilterKind.All)
        {
            return;
        }
        var splitByWhitspace = inputText != null ? SearchHelpers.SplitByWhitspaceAndSpecificSymbols(inputText) : Array.Empty<string>();
        AvailableIngredientAndRecipes.Filter = item => SearchFilters.FilterIngredientsOrRecipe(item, splitByWhitspace, _lastFilterKind);
    }

    public string? SearchFilter
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
            {
                FilterAvailableFeeds(value);
            }
        }
    }

    /// <inheritdoc />
    public void SetFilterKind(FilterKind kind)
    {
        if (_lastFilterKind != kind)
        {
            _lastFilterKind = kind;
            FilterAvailableFeeds(_searchText);
        }
    }

    private void OnRecipesChanged(IRecipesChangeMonitor monitor)
    {
        foreach (var removed in monitor.GetRemoved())
        {
            if (_adapterByGuid.TryGetValue(removed.Guid, out var adapter))
            {
                _availableFeeds.Remove(adapter);
            }
        }

        foreach (var added in monitor.GetAdded())
        {
            DebugCore.Assert(_adapterByGuid.ContainsKey(added.Guid), $"duplicate Guid on added item: {added.Guid}");

            var adapter = new RecipeForListAdapter(added);
            _adapterByGuid.Add(added.Guid, adapter);
            _availableFeeds.Add(adapter);
        }
    }

    private void OnIngredientsChanged(IIngredientsChangeMonitor monitor)
    {
        foreach (var removed in monitor.GetRemoved())
        {
            if (_adapterByGuid.TryGetValue(removed.Guid, out var adapter))
            {
                _availableFeeds.Remove(adapter);
            }
        }

        foreach (var added in monitor.GetAdded())
        {
            DebugCore.Assert(_adapterByGuid.ContainsKey(added.Guid), $"duplicate Guid on added item: {added.Guid}");

            IIngredientOrRecipeForListAdapter adapter = added.IsUserAdded
                ? new UserDefinedIngredientForListAdapter(added)
                : new ReferenceIngredientForListAdapter(added);
            _adapterByGuid.Add(added.Guid, adapter);
            _availableFeeds.Add(adapter);
        }
    }
}