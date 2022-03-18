using PRF.WPFCore;
using VetSolutionRatio.wpf.Views.RatioPanel.Components;

namespace VetSolutionRatio.wpf.Views.RatioPanel.SubPanels
{
    internal interface IAnimalSelectionViewModel
    {
    }

    internal sealed class AnimalSelectionViewModel : ViewModelBase, IAnimalSelectionViewModel
    {
        public IAnimalKindHoster AnimalKindHoster { get; }

        public AnimalSelectionViewModel(IAnimalKindHoster animalKindHoster)
        {
            AnimalKindHoster = animalKindHoster;
        }
        
        private string? _searchFilter;

        public string? SearchFilter
        {
            get => _searchFilter;
            set
            {
                if (SetProperty(ref _searchFilter, value))
                {
                    AnimalKindHoster.FilterAnimal(value);
                }
            }
        }
    }
}