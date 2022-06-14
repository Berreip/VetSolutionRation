using VetSolutionRation.wpf.Views.Adapter.FeedSelection;

namespace VetSolutionRation.wpf.Views.RatioPanel.CalculateRatio;

internal sealed partial class CalculateResultView : IResultView
{
    public CalculateResultView(ICalculateResultViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}