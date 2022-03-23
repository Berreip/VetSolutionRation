using VetSolutionRatio.wpf.Views.RatioPanel.Adapter.FeedSelection;

namespace VetSolutionRatio.wpf.Views.RatioPanel.SubPanels.ResultPanels.Verify;

internal sealed partial class VerifyResultView : IResultView
{
    public VerifyResultView(IVerifyResultViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}