using PRF.WPFCore;
using VetSolutionRatio.wpf.Views.RatioPanel.Adapter.FeedSelection;
using VetSolutionRatio.wpf.Views.RatioPanel.SubPanels.FeedSelection.Calculate;
using VetSolutionRatio.wpf.Views.RatioPanel.SubPanels.FeedSelection.Verify;

namespace VetSolutionRatio.wpf.Views.RatioPanel.SubPanels.FeedSelection;

internal interface IFeedSelectionViewModel
{
    IFeedSelectionModeAdapter[] AvailableFeedSelectionModes { get; }
}

internal sealed class FeedSelectionViewModel : ViewModelBase, IFeedSelectionViewModel
{
    private IFeedSelectionModeView _selectedFeedSelectionMode;
    public IFeedSelectionModeAdapter[] AvailableFeedSelectionModes { get; }

    // ReSharper disable SuggestBaseTypeForParameterInConstructor
    public FeedSelectionViewModel(CalculateRatiosView calculateRatiosView, VerifyRatiosView verifyRatiosView)
        // ReSharper restore SuggestBaseTypeForParameterInConstructor
    {
        AvailableFeedSelectionModes = new IFeedSelectionModeAdapter[]
        {
            new FeedSelectionModeAdapter(Properties.VetSolutionRatioRes.FeedSelectionModeVerifyRatiosTitle, verifyRatiosView),
            new FeedSelectionModeAdapter(Properties.VetSolutionRatioRes.FeedSelectionModeCalculateRatiosTitle, calculateRatiosView),
        };

        foreach (var adapter in AvailableFeedSelectionModes)
        {
            adapter.OnFeedSelectionModeSelected += OnFeedSelectionModeSelected;
        }

        _selectedFeedSelectionMode = AvailableFeedSelectionModes[0].View;
    }

    private void OnFeedSelectionModeSelected(IFeedSelectionModeAdapter adapter)
    {
        SelectedFeedSelectionMode = adapter.View;
    }

    public IFeedSelectionModeView SelectedFeedSelectionMode
    {
        get => _selectedFeedSelectionMode;
        private set => SetProperty(ref _selectedFeedSelectionMode, value);
    }
}