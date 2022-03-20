using System.Windows.Controls;
using VetSolutionRatio.wpf.Views.RatioPanel.SubPanels.AnimalSelection;

namespace VetSolutionRatio.wpf.Views.RatioPanel.SubPanels
{
    internal sealed partial class AnimalSelectionView : UserControl
    {
        public AnimalSelectionView(IAnimalSelectionViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}