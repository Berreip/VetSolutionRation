using VetSolutionRatio.wpf.Views.RatioPanel.Adapter.FeedSelection;

namespace VetSolutionRatio.wpf.Views.RatioPanel.SubPanels.ResultPanels.Calculate;

internal sealed partial class CalculateResultView : IResultView
{
    public CalculateResultView(ICalculateResultViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}