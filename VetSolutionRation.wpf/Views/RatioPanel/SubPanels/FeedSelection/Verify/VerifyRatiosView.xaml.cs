using VetSolutionRation.wpf.Views.RatioPanel.Adapter.FeedSelection;

namespace VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Verify;

internal sealed partial class VerifyRatiosView : IFeedSelectionModeView
{
    public IVerifyRatiosViewModel ViewModel { get; }
    
    public VerifyRatiosView(IVerifyRatiosViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
        ViewModel = vm;
    }
}