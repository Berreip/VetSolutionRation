using System.Windows.Controls;

namespace VetSolutionRatio.wpf.Views.RatioPanel.SubPanels.Results;

internal partial class ResultView : UserControl
{
    public ResultView(IResultViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}