using PRF.WPFCore;
using PRF.WPFCore.Commands;
using PRF.WPFCore.PopupManager;
using PRF.WPFCore.UiWorkerThread;
using VetSolutionRation.Common.Async;
using VetSolutionRation.wpf.Services;
using VetSolutionRation.wpf.Views.Popups;
using VetSolutionRation.wpf.Views.Popups.DuplicatesAndEditFeed;
using VetSolutionRation.wpf.Views.RatioPanel.Adapter.FeedSelection;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Adapters;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Calculate;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Verify;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.ResultPanels.Calculate;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.ResultPanels.Verify;
using VetSolutionRationLib.Models.Feed;

namespace VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection;

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
    private IFeedSelectionModeView _selectedFeedSelectionMode;
    private IResultView _selectedResultView;
    public IFeedSelectionModeAdapter[] AvailableFeedSelectionModes { get; }
    public IFeedProviderHoster FeedProviderHoster { get; }
    public IDelegateCommandLight<ReferenceFeedAdapter> SelectFeedCommand { get; }
    public IDelegateCommandLight<FeedAdapterBase> DuplicateFeedCommand { get; }
    public IDelegateCommandLight<CustomUserFeedAdapter> EditFeedCommand { get; }
    public IDelegateCommandLight<CustomUserFeedAdapter> DeleteFeedCommand { get; }

    public FeedSelectionViewModel(
        // ReSharper disable SuggestBaseTypeForParameterInConstructor
        CalculateRatiosView calculateRatiosView,
        VerifyRatiosView verifyRatiosView,
        CalculateResultView calculateResultView,
        VerifyResultView verifyResultView,
        // ReSharper restore SuggestBaseTypeForParameterInConstructor
        IFeedProviderHoster feedProviderHoster)
    {
        _calculateRatiosView = calculateRatiosView;
        _verifyRatiosView = verifyRatiosView;
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

        SelectFeedCommand = new DelegateCommandLight<FeedAdapterBase>(ExecuteSelectFeedCommand);
        DuplicateFeedCommand = new DelegateCommandLight<FeedAdapterBase>(ExecuteDuplicateFeedCommand);
        EditFeedCommand = new DelegateCommandLight<CustomUserFeedAdapter>(ExecuteEditFeedCommand);
        DeleteFeedCommand = new DelegateCommandLight<CustomUserFeedAdapter>(ExecuteDeleteFeedCommand);
    }

    private void ExecuteDeleteFeedCommand(CustomUserFeedAdapter obj)
    {
        // TODO PBO
    }

    private void ExecuteEditFeedCommand(CustomUserFeedAdapter obj)
    {
        // TODO PBO
    }

    private void ExecuteDuplicateFeedCommand(FeedAdapterBase feed)
    {
        var vm = new DuplicateAndEditFeedPopupViewModel(feed, OnDuplicateValidated);
        var view = new DuplicateAndEditFeedPopupView(vm);
        view.ShowDialog();
    }

    private void OnDuplicateValidated(IFeed newFeed)
    {
        var adapter = newFeed.CreateAdapter();
        // TODO PBO;
    }

    private void ExecuteSelectFeedCommand(FeedAdapterBase feedAdapter)
    {
        _calculateRatiosView.ViewModel.AddSelectedFeed(feedAdapter);
        _verifyRatiosView.ViewModel.AddSelectedFeed(feedAdapter);
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