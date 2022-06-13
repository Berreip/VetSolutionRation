﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using PRF.WPFCore.CustomCollections;
using PRF.WPFCore.UiWorkerThread;
using VetSolutionRation.Common.Async;
using VetSolutionRation.wpf.Services.Feed;
using VetSolutionRation.wpf.Services.PopupManager;
using VetSolutionRation.wpf.Views.Popups.DuplicatesAndEditFeed;
using VetSolutionRation.wpf.Views.Popups.RecipeConfiguration;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Adapters;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.Recipe;
using VetSolutionRationLib.Enums;

namespace VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Verify;

internal interface IVerifyRatiosViewModel
{
    void AddSelectedFeed(IFeedOrReciepe feedOrRecipe);
}

internal sealed class VerifyRatiosViewModel : ViewModelBase, IVerifyRatiosViewModel
{
    private readonly IRecipeCalculator _recipeCalculator;
    private readonly IFeedProvider _feedProvider;
    private readonly IPopupManagerLight _popupManagerLight;
    public ICollectionView SelectedFeedsForVerifyPanel { get; }
    private readonly ObservableCollectionRanged<IVerifyFeed> _selectedFeedForVerifyPanel;
    private readonly HashSet<IFeedOrReciepe> _alreadyAddedHash = new HashSet<IFeedOrReciepe>();

    public IDelegateCommandLight<FeedVerifySpecificAdapter> RemoveFromSelectedFeedsCommand { get; }

    public IDelegateCommandLight CreateRecipeCommand { get; }

    /// <inheritdoc />
    public VerifyRatiosViewModel(IRecipeCalculator recipeCalculator, IFeedProvider feedProvider, IPopupManagerLight popupManagerLight)
    {
        _recipeCalculator = recipeCalculator;
        _feedProvider = feedProvider;
        _popupManagerLight = popupManagerLight;
        SelectedFeedsForVerifyPanel = ObservableCollectionSource.GetDefaultView(out _selectedFeedForVerifyPanel);
        RemoveFromSelectedFeedsCommand = new DelegateCommandLight<FeedVerifySpecificAdapter>(ExecuteRemoveFromSelectedFeedsCommand);
        CreateRecipeCommand = new DelegateCommandLight(ExecuteCreateRecipeCommand, CanExecuteCreateRecipeCommand);
    }

    private async void ExecuteCreateRecipeCommand()
    {
        await AsyncWrapper.DispatchAndWrapAsync(async () =>
        {
            var recipeConfiguration = await UiThreadDispatcher.ExecuteOnUIAsync(() =>
            {
                var vm = new RecipeConfigurationPopupViewModel(_popupManagerLight, _feedProvider, _selectedFeedForVerifyPanel.Where(o => o.IsSelected).ToArray());
                _popupManagerLight.ShowDialog(() => vm, vmCreated => new RecipeConfigurationPopupView(vmCreated));
                return vm.ReciepedConfiguration;
            }).ConfigureAwait(false);

            if (recipeConfiguration != null)
            {
                //
                var recipe = _recipeCalculator.CreateNewReciepe(recipeConfiguration);
            
                // save it:
                _feedProvider.SaveReciepe(recipe);
            
                // do not remove the feed that leads to the reciepe if the user wants to create another one
                AddSelectedFeed(new RecipeAdapter(recipe));
            }

        }).ConfigureAwait(false);
    }

    private bool CanExecuteCreateRecipeCommand()
    {
        return _recipeCalculator.CouldCalculateRecipe(_selectedFeedForVerifyPanel);
    }

    /// <inheritdoc />
    public void AddSelectedFeed(IFeedOrReciepe feedOrRecipe)
    {
        if (_alreadyAddedHash.Add(feedOrRecipe))
        {
            switch (feedOrRecipe)
            {
                case IFeedAdapter feedAdapter:
                    _selectedFeedForVerifyPanel.Add(new FeedVerifySpecificAdapter(feedAdapter, FeedUnit.Kg, true));
                    break;
                case IRecipeAdapter recipe:
                    throw new NotImplementedException("TODO PBO");
                    // _selectedFeedForVerifyPanel.Add(new FeedVerifySpecificAdapter(feedAdapter, "kg", true));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(feedOrRecipe));
            }
            CreateRecipeCommand.RaiseCanExecuteChanged();
        }
    }
    
    private void ExecuteRemoveFromSelectedFeedsCommand(FeedVerifySpecificAdapter feed)
    {
        if(_alreadyAddedHash.Remove(feed.GetUnderlyingFeedAdapter()))
        {
            _selectedFeedForVerifyPanel.Remove(feed);
        }
    }
}