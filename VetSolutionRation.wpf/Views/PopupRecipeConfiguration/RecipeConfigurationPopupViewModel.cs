using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using PRF.WPFCore.CustomCollections;
using PRF.WPFCore.UiWorkerThread;
using VSR.Core.Helpers.Async;
using VSR.Core.Services;
using VSR.Enums;
using VSR.Models.Ingredients;
using VSR.WPF.Utils.Adapters.CalculationAdapters;
using VSR.WPF.Utils.Adapters.EditionRecipes;
using VSR.WPF.Utils.PopupManager;

namespace VetSolutionRation.wpf.Views.RecipeConfiguration;

internal sealed class RecipeConfigurationPopupViewModel : ViewModelBase, IPopupViewModel
{
    private readonly IPopupManagerLight _popupManager;
    private readonly IIngredientsManager _ingredientsManager;
    private readonly ObservableCollectionRanged<IngredientInRecipeCreationAdapter> _feedSelectedForRecipe;
    public ICollectionView SelectedIngredients { get; }
    private string? _recipeName;
    private bool _isDuplicatedLabel;

    public IDelegateCommandLight ValidateRecipeCreationCommand { get; }
    public IDelegateCommandLight CancelCreationCommand { get; }
    public IRecipeConfiguration? RecipeConfiguration { get; private set; }

    public RecipeConfigurationPopupViewModel(
        IPopupManagerLight popupManager,
        IIngredientsManager ingredientsManager, 
        IReadOnlyList<IAdapterInCalculation> selectedIngredientsAndRecipe)
    {
        _popupManager = popupManager;
        _ingredientsManager = ingredientsManager;

        var ingredients = RecipeConfigurationCalculator.GetAllIndividualIngredients(selectedIngredientsAndRecipe);
        SelectedIngredients = ObservableCollectionSource.GetDefaultView(ingredients.Select(o => new IngredientInRecipeCreationAdapter(o.Ingredient, o.Quantity)).ToArray(), out _feedSelectedForRecipe);

        ValidateRecipeCreationCommand = new DelegateCommandLight(ExecuteValidateRecipeCreationCommand, CanExecuteValidateRecipeCreationCommand);
        CancelCreationCommand = new DelegateCommandLight(ExecuteCancelCreationCommand);
        
        foreach (var adapter in _feedSelectedForRecipe)
        {
            adapter.OnQuantityChanged += RefreshPercentage;
        }
        RefreshPercentage();
    }

    private void RefreshPercentage()
    {
        var percentagesByAdapter = _feedSelectedForRecipe.NormalizedQuantityByIngredientAndGetPercentage();
        foreach (var adapter in _feedSelectedForRecipe)
        {
            adapter.Percentage = percentagesByAdapter[adapter];
        }
    }

    private void ExecuteCancelCreationCommand()
    {
        RecipeConfiguration = null;
        _popupManager.RequestClosing(this);
    }

    private bool CanExecuteValidateRecipeCreationCommand()
    {
        return !string.IsNullOrWhiteSpace(_recipeName) &&
               _isDuplicatedLabel == false &&
               _feedSelectedForRecipe.Count != 0 &&
               _feedSelectedForRecipe.All(o => o.IsValidForRecipe());
    }

    private async void ExecuteValidateRecipeCreationCommand()
    { 
        await AsyncWrapper.WrapAsync(async () =>
        {
            if (_recipeName == null)
            {
                throw new InvalidOperationException("Can execute should not let the cmd be executed if null");
            }

            RecipeConfiguration = _feedSelectedForRecipe.CalculateRecipeConfiguration(_recipeName);
            await UiThreadDispatcher.ExecuteOnUIAsync(() =>
            {
                _popupManager.RequestClosing(this);
            }).ConfigureAwait(false);
        }).ConfigureAwait(false);
    }

    public string? RecipeName
    {
        get => _recipeName;
        set
        {
            if (SetProperty(ref _recipeName, value))
            {
                RefreshValidity();
            }
        }
    }

    public bool IsDuplicatedLabel
    {
        get => _isDuplicatedLabel;
        private set => SetProperty(ref _isDuplicatedLabel, value);
    }

    private void RefreshValidity()
    {
        IsDuplicatedLabel = _recipeName != null && _ingredientsManager.TryGetIngredientByName(_recipeName, out _);
        ValidateRecipeCreationCommand.RaiseCanExecuteChanged();
    }
}

internal interface IRecipeConfiguration
{
    /// <summary>
    /// Returns the list of all ingredient recipe
    /// </summary>
    /// <returns></returns>
    IReadOnlyList<IIngredientForRecipe> GetIngredients();

    /// <summary>
    /// The recipe name
    /// </summary>
    string RecipeName { get; }

    /// <summary>
    /// The unit that define all ingredient in this recipe
    /// </summary>
    FeedUnit RecipeUnit { get; }
}

internal sealed class RecipeConfiguration : IRecipeConfiguration
{
    private readonly IReadOnlyList<IIngredientForRecipe> _feedForRecipeCreations;

    public RecipeConfiguration(string recipeName, IReadOnlyList<IIngredientForRecipe> feedForRecipeCreations, FeedUnit recipeUnit)
    {
        _feedForRecipeCreations = feedForRecipeCreations;
        RecipeUnit = recipeUnit;
        RecipeName = recipeName;
    }

    /// <inheritdoc />
    public IReadOnlyList<IIngredientForRecipe> GetIngredients() => _feedForRecipeCreations;

    /// <inheritdoc />
    public string RecipeName { get; }

    /// <inheritdoc />
    public FeedUnit RecipeUnit { get; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{RecipeName} ({RecipeUnit})[{string.Join(", ", _feedForRecipeCreations)}]";
    }
}