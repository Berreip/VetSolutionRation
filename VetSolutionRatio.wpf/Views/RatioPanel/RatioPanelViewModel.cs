using PRF.WPFCore;
using VetSolutionRatio.wpf.Views.RatioPanel.SubPanels;

namespace VetSolutionRatio.wpf.Views.RatioPanel
{
    internal interface IRatioPanelViewModel
    {
    }

    internal sealed class RatioPanelViewModel : ViewModelBase, IRatioPanelViewModel
    {
        public RatioPanelViewModel(AnimalSelectionView animalSelectionView)
        {
            AnimalSelectionView = animalSelectionView;
        }

        public AnimalSelectionView AnimalSelectionView { get; }
    }
}