using System;
using System.ComponentModel;
using System.Linq;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using PRF.WPFCore.CustomCollections;
using PRF.WPFCore.UiWorkerThread;
using VetSolutionRation.wpf.Views.PopupRecipeConfiguration;
using VSR.Core.Helpers.Async;
using VSR.Core.Services;
using VSR.Models.Recipe;
using VSR.WPF.Utils.Adapters.EditionRecipes;
using VSR.WPF.Utils.PopupManager;

namespace VetSolutionRation.wpf.Views.PopupRecipeEdition;

internal interface IRecipeEditionPopupViewModel
{
}

internal sealed class RecipeEditionPopupViewModel : ViewModelBase, IRecipeEditionPopupViewModel, IPopupViewModel
{
    private readonly IPopupManagerLight _popupManager;
    private readonly IIngredientsManager _ingredientsManager;
    private string? _recipeName;
    private bool _isDuplicatedLabel;

    private readonly ObservableCollectionRanged<IngredientInRecipeCreationAdapter> _ingredientsInEditedRecipe;
    public ICollectionView IngredientsInEditedRecipe { get; }
    
    public IDelegateCommandLight ValidateRecipeEditionCommand { get; }
    public IDelegateCommandLight CancelEditionCommand { get; }
    public IRecipeConfiguration? RecipeConfiguration { get; private set; }
    
    public RecipeEditionPopupViewModel(IRecipe recipe,
        IPopupManagerLight popupManager,
        IIngredientsManager ingredientsManager)
    {
        _popupManager = popupManager;
        _ingredientsManager = ingredientsManager;
        _recipeName = recipe.RecipeName;
        ValidateRecipeEditionCommand = new DelegateCommandLight(ExecuteValidateRecipeEditionCommand, CanExecuteValidateRecipeEditionCommand);
        CancelEditionCommand = new DelegateCommandLight(ExecuteCancelEditionCommand);
        
        IngredientsInEditedRecipe = ObservableCollectionSource
            .GetDefaultView(recipe.IngredientsForRecipe
                .Select(o => new IngredientInRecipeCreationAdapter(o.Ingredient, (int)Math.Round(o.Percentage*100))) // round percentage to get int only
                .ToArray(),
                out _ingredientsInEditedRecipe);
        
        foreach (var adapter in _ingredientsInEditedRecipe)
        {
            adapter.OnQuantityChanged += RefreshPercentage;
        }
        RefreshPercentage();
    }

    private void RefreshPercentage()
    {
        var percentagesByAdapter = _ingredientsInEditedRecipe.NormalizedQuantityByIngredientAndGetPercentage();
        foreach (var adapter in _ingredientsInEditedRecipe)
        {
            adapter.Percentage = percentagesByAdapter[adapter];
        }
    }
    
    private void ExecuteCancelEditionCommand()
    {
        RecipeConfiguration = null;
        _popupManager.RequestClosing(this);
    }
    private bool CanExecuteValidateRecipeEditionCommand()
    {
        return !string.IsNullOrWhiteSpace(_recipeName) &&
               _isDuplicatedLabel == false &&
               _ingredientsInEditedRecipe.Count != 0 &&
               _ingredientsInEditedRecipe.All(o => o.IsValidForRecipe());
    }
    
    private async void ExecuteValidateRecipeEditionCommand()
    {
        await AsyncWrapper.WrapAsync(async () =>
        {
            if (_recipeName == null)
            {
                throw new InvalidOperationException("Can execute should not let the cmd be executed if null");
            }

            RecipeConfiguration = _ingredientsInEditedRecipe.CalculateRecipeConfiguration(_recipeName);
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
        ValidateRecipeEditionCommand.RaiseCanExecuteChanged();
    }
}