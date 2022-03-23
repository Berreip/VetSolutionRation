using System.Windows.Controls;

namespace VetSolutionRation.wpf.Views.RatioPanel.SubPanels.AnimalSelection
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