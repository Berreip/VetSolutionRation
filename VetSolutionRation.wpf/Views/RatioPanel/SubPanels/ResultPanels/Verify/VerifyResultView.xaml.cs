using VetSolutionRation.wpf.Views.RatioPanel.Adapter.FeedSelection;

namespace VetSolutionRation.wpf.Views.RatioPanel.SubPanels.ResultPanels.Verify;

internal sealed partial class VerifyResultView : IResultView
{
    public VerifyResultView(IVerifyResultViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}