using PRF.WPFCore;
using PRF.WPFCore.Commands;
using VetSolutionRation.wpf.Services;
using VetSolutionRation.wpf.Views.RatioPanel.Adapter.FeedSelection;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Adapters;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Calculate;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Verify;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.ResultPanels.Calculate;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.ResultPanels.Verify;

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
        
        SelectFeedCommand = new DelegateCommandLight<ReferenceFeedAdapter>(ExecuteSelectFeedCommand);
        DuplicateFeedCommand = new DelegateCommandLight<FeedAdapterBase>(ExecuteDuplicateFeedCommand);
        EditFeedCommand = new DelegateCommandLight<FeedAdapterBase>(ExecuteEditFeedCommand);
        DeleteFeedCommand = new DelegateCommandLight<FeedAdapterBase>(ExecuteDeleteFeedCommand);
    }

    private void ExecuteDeleteFeedCommand(FeedAdapterBase obj)
    {
        // TODO PBO
    }

    private void ExecuteEditFeedCommand(FeedAdapterBase obj)
    {
        // TODO PBO
    }

    private void ExecuteDuplicateFeedCommand(FeedAdapterBase obj)
    {
        // TODO PBO
    }

    private void ExecuteSelectFeedCommand(ReferenceFeedAdapter feedAdapter)
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