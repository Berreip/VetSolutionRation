using VetSolutionRation.wpf.Views.RatioPanel.Adapter.FeedSelection;

namespace VetSolutionRation.wpf.Views.RatioPanel.SubPanels.ResultPanels.Calculate;

internal sealed partial class CalculateResultView : IResultView
{
    public CalculateResultView(ICalculateResultViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}