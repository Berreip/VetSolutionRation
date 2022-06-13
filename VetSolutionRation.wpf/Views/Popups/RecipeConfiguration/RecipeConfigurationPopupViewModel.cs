using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using PRF.WPFCore.CustomCollections;
using VetSolutionRation.wpf.Services.Feed;
using VetSolutionRation.wpf.Services.PopupManager;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Adapters;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models.Feed;
using VetSolutionRationLib.Models.Recipe;

namespace VetSolutionRation.wpf.Views.Popups.RecipeConfiguration;

internal sealed class RecipeConfigurationPopupViewModel : ViewModelBase, IPopupViewModel
{
    private readonly IPopupManagerLight _popupManager;
    private readonly IFeedProvider _feedProvider;
    private readonly ObservableCollectionRanged<FeedForRecipeCreationAdapter> _feedSelectedForRecipe;
    public ICollectionView SelectedFeedsCollection { get; }
    private string? _reciepeName;
    private bool _isDuplicatedLabel;

    public IDelegateCommandLight ValidateRecipeCreationCommand { get; }
    public IDelegateCommandLight CancelCreationCommand { get; }
    public IRecipeConfiguration? ReciepedConfiguration { get; private set; }

    public RecipeConfigurationPopupViewModel(IPopupManagerLight popupManager, IFeedProvider feedProvider, IReadOnlyList<IVerifyFeed> selectedFeeds)
    {
        _popupManager = popupManager;
        _feedProvider = feedProvider;

        SelectedFeedsCollection = ObservableCollectionSource.GetDefaultView(selectedFeeds.Select(o => new FeedForRecipeCreationAdapter(o)).ToArray(), out _feedSelectedForRecipe);

        ValidateRecipeCreationCommand = new DelegateCommandLight(ExecuteValidateRecipeCreationCommand, CanExecuteValidateRecipeCreationCommand);
        CancelCreationCommand = new DelegateCommandLight(ExecuteCancelCreationCommand);
    }

    private void ExecuteCancelCreationCommand()
    {
        ReciepedConfiguration = null;
        _popupManager.RequestClosing(this);
    }

    private bool CanExecuteValidateRecipeCreationCommand()
    {
        return !string.IsNullOrWhiteSpace(_reciepeName) &&
               _isDuplicatedLabel == false &&
               _feedSelectedForRecipe.Count != 0 &&
               _feedSelectedForRecipe.All(o => o.IsValidForRecipe());
    }

    private void ExecuteValidateRecipeCreationCommand()
    {
        if (_reciepeName == null)
        {
            throw new InvalidOperationException("Can execute should not let the cmd be executed if null");
        }
        ReciepedConfiguration = _feedSelectedForRecipe.CalculateRecipeConfiguration(_reciepeName);
        _popupManager.RequestClosing(this);
    }

    public string? ReciepeName
    {
        get => _reciepeName;
        set
        {
            if (SetProperty(ref _reciepeName, value))
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
        IsDuplicatedLabel = _reciepeName != null && _feedProvider.ContainsRecipeName(_reciepeName);
        ValidateRecipeCreationCommand.RaiseCanExecuteChanged();
    }
}

internal interface IFeedForRecipeCreationAdapter
{
    string Name { get; }
    IFeedQuantityAdapter FeedQuantity { get; }
    IReadOnlyList<(IFeedAdapter Feed, double Percentage)> GetUnderlyingFeeds();
}

internal sealed class FeedForRecipeCreationAdapter : ViewModelBase, IFeedForRecipeCreationAdapter
{
    private readonly IVerifyFeed _feed;

    public FeedForRecipeCreationAdapter(IVerifyFeed feed)
    {
        _feed = feed;
        Name = feed.Name;
        // copy the adapter to avoid multipe reference pointing to the same adapter
        FeedQuantity = new FeedQuantityAdapter(feed.FeedQuantity.Unit, feed.FeedQuantity.Quantity);
    }

    public string Name { get; }
    public IFeedQuantityAdapter FeedQuantity { get; }

    public bool IsValidForRecipe()
    {
        // should have a positive quantity
        return FeedQuantity.Quantity > 0d;
    }

    public IReadOnlyList<(IFeedAdapter Feed, double Percentage)> GetUnderlyingFeeds()
    {
        // simple case: one unique ingredient
        return new[] { (_feed.GetUnderlyingFeedAdapter(), 1d) };
    }
}


internal sealed class FeedForRecipe : IFeedForRecipe
{
    public FeedForRecipe(double percentage, IFeedAdapter feedAdapter)
    {
        Percentage = percentage;
        Ingredient = feedAdapter.GetUnderlyingFeed();
    }

    /// <inheritdoc />
    public double Percentage { get; }

    /// <inheritdoc />
    public IFeed Ingredient { get; }
}

internal interface IRecipeConfiguration
{
    /// <summary>
    /// Returns the list of all ingredient recipe
    /// </summary>
    /// <returns></returns>
    IReadOnlyList<IFeedForRecipe> GetIngredients();
    
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
    private readonly IReadOnlyList<IFeedForRecipe> _feedForRecipeCreations;

    public RecipeConfiguration(string recipeName, IReadOnlyList<IFeedForRecipe> feedForRecipeCreations, FeedUnit recipeUnit)
    {
        _feedForRecipeCreations = feedForRecipeCreations;
        RecipeUnit = recipeUnit;
        RecipeName = recipeName;
    }

    /// <inheritdoc />
    public IReadOnlyList<IFeedForRecipe> GetIngredients() => _feedForRecipeCreations;

    /// <inheritdoc />
    public string RecipeName { get; }

    /// <inheritdoc />
    public FeedUnit RecipeUnit { get; }
}