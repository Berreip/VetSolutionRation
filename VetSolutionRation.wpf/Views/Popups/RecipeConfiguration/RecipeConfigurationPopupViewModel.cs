using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using PRF.WPFCore.CustomCollections;
using PRF.WPFCore.UiWorkerThread;
using VetSolutionRation.Common.Async;
using VetSolutionRation.wpf.Services.PopupManager;
using VetSolutionRation.wpf.Services.Saves;
using VetSolutionRation.wpf.Views.Popups.RecipeConfiguration.Adapters;
using VetSolutionRation.wpf.Views.RatioPanel.Recipe;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models.Recipe;

namespace VetSolutionRation.wpf.Views.Popups.RecipeConfiguration;

internal sealed class RecipeConfigurationPopupViewModel : ViewModelBase, IPopupViewModel
{
    private readonly IPopupManagerLight _popupManager;
    private readonly IFeedProvider _feedProvider;
    private readonly ObservableCollectionRanged<FeedForRecipeCreationAdapter> _feedSelectedForRecipe;
    public ICollectionView SelectedFeedsCollection { get; }
    private string? _recipeName;
    private bool _isDuplicatedLabel;

    public IDelegateCommandLight ValidateRecipeCreationCommand { get; }
    public IDelegateCommandLight CancelCreationCommand { get; }
    public IRecipeConfiguration? RecipeConfiguration { get; private set; }

    public RecipeConfigurationPopupViewModel(
        IPopupManagerLight popupManager,
        IFeedProvider feedProvider, 
        IReadOnlyList<IFeedThatCouldBeAddedIntoRecipe> selectedFeeds)
    {
        _popupManager = popupManager;
        _feedProvider = feedProvider;

        var allFeeds = RecipeConfigurationCalculator.GetAllIndividualFeeds(selectedFeeds);
        SelectedFeedsCollection = ObservableCollectionSource.GetDefaultView(allFeeds.Select(o => new FeedForRecipeCreationAdapter(o)).ToArray(), out _feedSelectedForRecipe);

        ValidateRecipeCreationCommand = new DelegateCommandLight(ExecuteValidateRecipeCreationCommand, CanExecuteValidateRecipeCreationCommand);
        CancelCreationCommand = new DelegateCommandLight(ExecuteCancelCreationCommand);
        
        foreach (var adapter in _feedSelectedForRecipe)
        {
            adapter.FeedQuantity.OnQuantityChanged += RefreshPercentage;
        }
        RefreshPercentage();
    }

    private void RefreshPercentage()
    {
        var percentagesByAdapter = _feedSelectedForRecipe.NormalizedQuantityByFeedAndGetPercentage();
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
        IsDuplicatedLabel = _recipeName != null && _feedProvider.ContainsRecipeName(_recipeName);
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
}