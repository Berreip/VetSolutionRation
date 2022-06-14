using VetSolutionRation.wpf.Views.Adapter.FeedSelection;

namespace VetSolutionRation.wpf.Views.RatioPanel.VerifyRatio;

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