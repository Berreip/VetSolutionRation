using System;
using System.Windows.Forms;
using PRF.Utils.CoreComponents.Diagnostic;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using VetSolutionRation.Common.Async;
using VetSolutionRation.wpf.Services;
using VetSolutionRation.wpf.Services.PopupManager;
using VetSolutionRation.wpf.Services.Saves;
using VetSolutionRation.wpf.Views.Adapter;
using VetSolutionRation.wpf.Views.Adapter.Feeds;
using VetSolutionRation.wpf.Views.Adapter.FeedSelection;
using VetSolutionRation.wpf.Views.Popups.DuplicatesAndEditFeed;
using VetSolutionRation.wpf.Views.RatioPanel.CalculateRatio;
using VetSolutionRation.wpf.Views.RatioPanel.VerifyRatio;

namespace VetSolutionRation.wpf.Views.RatioPanel.FeedSelection;

internal interface IFeedSelectionViewModel
{
    IFeedSelectionModeAdapter[] AvailableFeedSelectionModes { get; }
    IResultView SelectedResultView { get; }
    IFeedSelectionModeView SelectedFeedSelectionMode { get; }
}

internal sealed class FeedSelectionViewModel : ViewModelBase, IFeedSelectionViewModel
{
    private readonly CalculateRatiosView _calculateRatiosView;
    private readonly VerifyRatiosView _verifyRatiosView;
    private readonly IFeedProvider _feedProvider;
    private readonly IPopupManagerLight _popupManagerLight;
    private IFeedSelectionModeView _selectedFeedSelectionMode;
    private IResultView _selectedResultView;
    public IFeedSelectionModeAdapter[] AvailableFeedSelectionModes { get; }
    public IFeedProviderHoster FeedProviderHoster { get; }
    public IDelegateCommandLight<IFeedOrRecipeAdapter?> SelectFeedCommand { get; }
    public IDelegateCommandLight<IFeedAdapter> DuplicateFeedCommand { get; }
    public IDelegateCommandLight<CustomUserFeedAdapter> EditFeedCommand { get; }
    public IDelegateCommandLight<CustomUserFeedAdapter> DeleteFeedCommand { get; }
    public IDelegateCommandLight<IRecipeAdapter?> EditRecipeCommand { get; }
    public IDelegateCommandLight<IRecipeAdapter?> DuplicateRecipeCommand { get; }
    public IDelegateCommandLight<IRecipeAdapter?> DeleteRecipeCommand { get; }

    public FeedSelectionViewModel(
        // ReSharper disable SuggestBaseTypeForParameterInConstructor
        CalculateRatiosView calculateRatiosView,
        VerifyRatiosView verifyRatiosView,
        CalculateResultView calculateResultView,
        VerifyResultView verifyResultView,
        // ReSharper restore SuggestBaseTypeForParameterInConstructor
        IFeedProvider feedProvider,
        IFeedProviderHoster feedProviderHoster,
        IPopupManagerLight popupManagerLight)
    {
        _calculateRatiosView = calculateRatiosView;
        _verifyRatiosView = verifyRatiosView;
        _feedProvider = feedProvider;
        _popupManagerLight = popupManagerLight;
        FeedProviderHoster = feedProviderHoster;
        AvailableFeedSelectionModes = new IFeedSelectionModeAdapter[]
        {
            new FeedSelectionModeAdapter(Properties.VetSolutionRatioRes.FeedSelectionModeVerifyRatiosTitle, verifyRatiosView, verifyResultView),
            new FeedSelectionModeAdapter(Properties.VetSolutionRatioRes.FeedSelectionModeCalculateRatiosTitle, calculateRatiosView, calculateResultView),
        };

        foreach (var adapter in AvailableFeedSelectionModes)
        {
            adapter.OnFeedSelectionModeSelected += OnFeedSelectionModeSelected;
        }

        _selectedFeedSelectionMode = AvailableFeedSelectionModes[0].SelectionView;
        _selectedResultView = AvailableFeedSelectionModes[0].ResultView;
        AvailableFeedSelectionModes[0].IsSelected = true;

        SelectFeedCommand = new DelegateCommandLight<IFeedOrRecipeAdapter?>(ExecuteSelectFeedCommand);
        DuplicateFeedCommand = new DelegateCommandLight<IFeedAdapter?>(ExecuteDuplicateFeedCommand);
        EditFeedCommand = new DelegateCommandLight<CustomUserFeedAdapter?>(ExecuteEditFeedCommand);
        DeleteFeedCommand = new DelegateCommandLight<CustomUserFeedAdapter?>(ExecuteDeleteFeedCommand);
        EditRecipeCommand = new DelegateCommandLight<IRecipeAdapter?>(ExecuteEditRecipeCommand);
        DuplicateRecipeCommand = new DelegateCommandLight<IRecipeAdapter?>(ExecuteDuplicateRecipeCommand);
        DeleteRecipeCommand = new DelegateCommandLight<IRecipeAdapter?>(ExecuteDeleteRecipeCommand);
    }

    private void ExecuteEditRecipeCommand(IRecipeAdapter? recipe)
    {
        AsyncWrapper.Wrap(() =>
        {
            if (recipe == null)
            {
                return;
            }

            DebugCore.Fail("TODO edit à faire");
        });
    }

    private void ExecuteDuplicateRecipeCommand(IRecipeAdapter? recipe)
    {
        AsyncWrapper.Wrap(() =>
        {
            if (recipe == null)
            {
                return;
            }

            DebugCore.Fail("TODO edit à faire");
        });
    }

    private void ExecuteDeleteRecipeCommand(IRecipeAdapter? recipe)
    {
        AsyncWrapper.Wrap(() =>
        {
            if (recipe == null)
            {
                return;
            }

            DebugCore.Fail("TODO edit à faire");
        });
    }

    private void ExecuteDeleteFeedCommand(CustomUserFeedAdapter? customUserFeed)
    {
        AsyncWrapper.Wrap(() =>
        {
            if (customUserFeed != null && MessageBox.Show(@$"Voulez vous vraiment supprimer l'aliment {customUserFeed.Name} ? ", @"Confirmation", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                _feedProvider.DeleteCustomFeedAndSave(customUserFeed.GetUnderlyingCustomFeed());
            }
        });
    }

    private void ExecuteEditFeedCommand(CustomUserFeedAdapter? feed)
    {
        AsyncWrapper.Wrap(() => ShowEditAndDuplicateWindow(feed, FeedEditionMode.Edition));
    }

    private void ExecuteDuplicateFeedCommand(IFeedAdapter? feed)
    {
        AsyncWrapper.Wrap(() => ShowEditAndDuplicateWindow(feed, FeedEditionMode.Duplication));
    }

    private void ShowEditAndDuplicateWindow(IFeedAdapter? feed, FeedEditionMode mode)
    {
        if (feed != null)
        {
            _popupManagerLight.ShowDialog(
                () => new DuplicateAndEditFeedPopupViewModel(_popupManagerLight, _feedProvider, feed, mode),
                vm => new DuplicateAndEditFeedPopupView(vm));
        }
    }

    private void ExecuteSelectFeedCommand(IFeedOrRecipeAdapter? feedOrRecipeAdapter)
    {
        if (feedOrRecipeAdapter != null)
        {
            switch (feedOrRecipeAdapter)
            {
                case IFeedAdapter feedAdapter:
                    _calculateRatiosView.ViewModel.AddSelectedFeed(feedAdapter);
                    _verifyRatiosView.ViewModel.AddSelectedFeed(feedAdapter.GetUnderlyingFeed());
                    break;
                case RecipeAdapter recipeAdapter:
                    _verifyRatiosView.ViewModel.AddSelectedFeed(recipeAdapter.GetUnderlyingRecipeModel());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(feedOrRecipeAdapter));
            }
        }
    }

    private void OnFeedSelectionModeSelected(IFeedSelectionModeAdapter adapter)
    {
        SelectedFeedSelectionMode = adapter.SelectionView;
        SelectedResultView = adapter.ResultView;
    }

    public IFeedSelectionModeView SelectedFeedSelectionMode
    {
        get => _selectedFeedSelectionMode;
        private set => SetProperty(ref _selectedFeedSelectionMode, value);
    }

    public IResultView SelectedResultView
    {
        get => _selectedResultView;
        private set => SetProperty(ref _selectedResultView, value);
    }
}