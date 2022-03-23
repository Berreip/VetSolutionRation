using PRF.WPFCore;
using VetSolutionRatio.wpf.Views.RatioPanel.SubPanels;
using VetSolutionRatio.wpf.Views.RatioPanel.SubPanels.AnimalSelection;
using VetSolutionRatio.wpf.Views.RatioPanel.SubPanels.FeedSelection;
using VetSolutionRatio.wpf.Views.RatioPanel.SubPanels.Results;

namespace VetSolutionRatio.wpf.Views.RatioPanel
{
    internal interface IRatioPanelViewModel
    {
    }

    internal sealed class RatioPanelViewModel : ViewModelBase, IRatioPanelViewModel
    {
        public RatioPanelViewModel(
            AnimalSelectionView animalSelectionView,
            FeedSelectionView feedSelectionView,
            ResultView resultView)
        {
            AnimalSelectionView = animalSelectionView;
            FeedSelectionView = feedSelectionView;
            ResultView = resultView;
        }

        public AnimalSelectionView AnimalSelectionView { get; }
        public FeedSelectionView FeedSelectionView { get; }
        public ResultView ResultView { get; }
    }
}