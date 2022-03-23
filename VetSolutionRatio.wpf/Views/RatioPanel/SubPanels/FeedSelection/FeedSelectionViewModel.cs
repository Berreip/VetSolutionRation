using PRF.WPFCore;
using VetSolutionRatio.wpf.Views.RatioPanel.Adapter.FeedSelection;
using VetSolutionRatio.wpf.Views.RatioPanel.SubPanels.FeedSelection.Calculate;
using VetSolutionRatio.wpf.Views.RatioPanel.SubPanels.FeedSelection.Verify;
using VetSolutionRatio.wpf.Views.RatioPanel.SubPanels.ResultPanels.Calculate;
using VetSolutionRatio.wpf.Views.RatioPanel.SubPanels.ResultPanels.Verify;

namespace VetSolutionRatio.wpf.Views.RatioPanel.SubPanels.FeedSelection;

internal interface IFeedSelectionViewModel
{
    IFeedSelectionModeAdapter[] AvailableFeedSelectionModes { get; }
    IResultView SelectedResultView { get; }
}

internal sealed class FeedSelectionViewModel : ViewModelBase, IFeedSelectionViewModel
{
    private IFeedSelectionModeView _selectedFeedSelectionMode;
    private IResultView _selectedResultView;
    public IFeedSelectionModeAdapter[] AvailableFeedSelectionModes { get; }

    // ReSharper disable SuggestBaseTypeForParameterInConstructor
    public FeedSelectionViewModel(CalculateRatiosView calculateRatiosView,
            VerifyRatiosView verifyRatiosView,
            CalculateResultView calculateResultView,
            VerifyResultView verifyResultView)
        // ReSharper restore SuggestBaseTypeForParameterInConstructor
    {
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