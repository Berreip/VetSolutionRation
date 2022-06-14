using PRF.WPFCore;
using VetSolutionRation.wpf.Views.RatioPanel.AnimalSelection;
using VetSolutionRation.wpf.Views.RatioPanel.FeedSelection;

namespace VetSolutionRation.wpf.Views.RatioPanel
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