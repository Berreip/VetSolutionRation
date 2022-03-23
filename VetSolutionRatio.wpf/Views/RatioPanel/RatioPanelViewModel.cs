using PRF.WPFCore;
using VetSolutionRatio.wpf.Views.RatioPanel.SubPanels.AnimalSelection;
using VetSolutionRatio.wpf.Views.RatioPanel.SubPanels.FeedSelection;

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
            IFeedSelectionViewModel feedSelectionViewModel)
        {
            AnimalSelectionView = animalSelectionView;
            FeedSelectionView = feedSelectionView;
            FeedSelectionViewModel = feedSelectionViewModel;
        }

        public AnimalSelectionView AnimalSelectionView { get; }
        public FeedSelectionView FeedSelectionView { get; }
        public IFeedSelectionViewModel FeedSelectionViewModel { get; }
    }
}