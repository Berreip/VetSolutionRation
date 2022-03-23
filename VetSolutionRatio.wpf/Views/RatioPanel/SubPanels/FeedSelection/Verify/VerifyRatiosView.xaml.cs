using VetSolutionRatio.wpf.Views.RatioPanel.Adapter.FeedSelection;

namespace VetSolutionRatio.wpf.Views.RatioPanel.SubPanels.FeedSelection.Verify;

internal sealed partial class VerifyRatiosView : IFeedSelectionModeView
{
    public VerifyRatiosView(IVerifyRatiosViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}