using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using PRF.WPFCore.CustomCollections;
using PRF.WPFCore.UiWorkerThread;
using VetSolutionRation.wpf.Views.PopupRecipeConfiguration;
using VSR.Core.Helpers.Async;
using VSR.Core.Services;
using VSR.Models.Ingredients;
using VSR.Models.Recipe;
using VSR.WPF.Utils.Adapters.CalculationAdapters;
using VSR.WPF.Utils.PopupManager;

namespace VetSolutionRation.wpf.Views.Calculation;

internal interface ICalculationViewModel
{
    void AddSelectedIngredient(IIngredient ingredient);
    void AddSelectedRecipe(IRecipe recipe);
}

internal sealed class CalculationViewModel : ViewModelBase, ICalculationViewModel
{
    private readonly IRecipeCalculator _recipeCalculator;
    private readonly IIngredientsManager _ingredientsManager;
    private readonly IPopupManagerLight _popupManagerLight;
    public ICollectionView IngredientAndRecipeInCalculationPanel { get; }
    private readonly ObservableCollectionRanged<IAdapterInCalculation> _ingredientAndRecipeInCalculationPanel;
    private readonly HashSet<Guid> _alreadyAddedHash = new HashSet<Guid>();

    public IDelegateCommandLight<IngredientInCalculationAdapter> RemoveFromSelectedIngredientCommand { get; }
    public IDelegateCommandLight CreateRecipeCommand { get; }

    /// <inheritdoc />
    public CalculationViewModel(IRecipeCalculator recipeCalculator, IIngredientsManager ingredientsManager, IPopupManagerLight popupManagerLight)
    {
        _recipeCalculator = recipeCalculator;
        _ingredientsManager = ingredientsManager;
        _popupManagerLight = popupManagerLight;
        IngredientAndRecipeInCalculationPanel = ObservableCollectionSource.GetDefaultView(out _ingredientAndRecipeInCalculationPanel);

        RemoveFromSelectedIngredientCommand = new DelegateCommandLight<IngredientInCalculationAdapter>(ExecuteRemoveFromSelectedFeedsCommand);
        CreateRecipeCommand = new DelegateCommandLight(ExecuteCreateRecipeCommand, CanExecuteCreateRecipeCommand);
    }

    private async void ExecuteCreateRecipeCommand()
    {
        await AsyncWrapper.DispatchAndWrapAsync(async () =>
        {
            var recipeConfiguration = await UiThreadDispatcher.ExecuteOnUIAsync(() =>
            {
                var vm = new RecipeConfigurationPopupViewModel(
                    _popupManagerLight,
                    _ingredientsManager,
                    _ingredientAndRecipeInCalculationPanel.Where(o => o.IsSelected).ToArray());
                _popupManagerLight.ShowDialog(() => vm, vmCreated => new RecipeConfigurationPopupView(vmCreated));
                return vm.RecipeConfiguration;
            }).ConfigureAwait(false);

            if (recipeConfiguration != null)
            {
                //
                var recipeCandidate = _recipeCalculator.CreateNewRecipe(recipeConfiguration);

                // save it:
                var createdRecipes = _ingredientsManager.AddRecipes(new[] { recipeCandidate });

                // do not remove the feed that leads to the recipe if the user wants to create another one
                foreach (var recipe in createdRecipes)
                {
                    AddSelectedRecipe(recipe);
                }
            }
        }).ConfigureAwait(false);
    }

    private bool CanExecuteCreateRecipeCommand()
    {
        return _recipeCalculator.CouldCalculateRecipe(_ingredientAndRecipeInCalculationPanel);
    }

    /// <inheritdoc />
    public void AddSelectedIngredient(IIngredient ingredient)
    {
        if (_alreadyAddedHash.Add(ingredient.Guid))
        {
            _ingredientAndRecipeInCalculationPanel.Add(new IngredientInCalculationAdapter(ingredient, OnIsSelectedChanged));
            CreateRecipeCommand.RaiseCanExecuteChanged();
        }
    }

    /// <inheritdoc />
    public void AddSelectedRecipe(IRecipe recipe)
    {
        if (_alreadyAddedHash.Add(recipe.Guid))
        {
            _ingredientAndRecipeInCalculationPanel.Add(new RecipeInCalculationAdapter(recipe));
            CreateRecipeCommand.RaiseCanExecuteChanged();
        }
    }

    private void OnIsSelectedChanged(bool isSelected)
    {
        CreateRecipeCommand.RaiseCanExecuteChanged();
    }

    private void ExecuteRemoveFromSelectedFeedsCommand(IngredientInCalculationAdapter feed)
    {
        if (_alreadyAddedHash.Remove(feed.Guid))
        {
            _ingredientAndRecipeInCalculationPanel.Remove(feed);
            CreateRecipeCommand.RaiseCanExecuteChanged();
        }
    }
}