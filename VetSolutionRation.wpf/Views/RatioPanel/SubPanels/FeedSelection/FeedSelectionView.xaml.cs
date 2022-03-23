using System.Windows.Controls;

namespace VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection;

internal partial class FeedSelectionView : UserControl
{
    public FeedSelectionView(IFeedSelectionViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}