using VetSolutionRation.wpf.Views.RatioPanel.Adapter.FeedSelection;

namespace VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Calculate;

internal sealed partial class CalculateRatiosView : IFeedSelectionModeView
{
    public CalculateRatiosView(ICalculateRatiosViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}