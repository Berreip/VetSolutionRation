using VetSolutionRation.wpf.Views.Adapter.FeedSelection;

namespace VetSolutionRation.wpf.Views.RatioPanel.VerifyRatio;

internal sealed partial class VerifyResultView : IResultView
{
    public VerifyResultView(IVerifyResultViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}