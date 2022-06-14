using System.Windows.Controls;

namespace VetSolutionRation.wpf.Views.RatioPanel.FeedSelection;

internal sealed partial class FeedSelectionView : UserControl
{
    public FeedSelectionView(IFeedSelectionViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}