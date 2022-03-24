using PRF.WPFCore;
using VetSolutionRation.wpf.Properties;
using VetSolutionRation.wpf.Views.RatioPanel.Components;

namespace VetSolutionRation.wpf.Views.RatioPanel.SubPanels.AnimalSelection
{
    internal interface IAnimalSelectionViewModel
    {
    }

    internal sealed class AnimalSelectionViewModel : ViewModelBase, IAnimalSelectionViewModel
    {
        private string? _searchFilter;
        private bool _isDropDownOpen;
        
        public IAnimalAdaptersHoster AnimalAdaptersHoster { get; }

        public AnimalSelectionViewModel(IAnimalAdaptersHoster animalAdaptersHoster)
        {
            AnimalAdaptersHoster = animalAdaptersHoster;
        }
        

        public string? SearchFilter
        {
            get => _searchFilter;
            set
            {
                if (SetProperty(ref _searchFilter, value))
                {
                    IsDropDownOpen = true;
                    AnimalAdaptersHoster.FilterAnimal(value);
                }
            }
        }

        public string Hint => VetSolutionRatioRes.View_AnimalSelection_Search_Hint;

        public string Tooltip => VetSolutionRatioRes.View_AnimalSelection_Search_Tooltip;

        public bool IsDropDownOpen
        {
            get => _isDropDownOpen;
            set => SetProperty(ref _isDropDownOpen, value);
        }
    }
}