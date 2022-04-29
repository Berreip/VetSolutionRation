using PRF.WPFCore;
using VetSolutionRation.wpf.Views.RatioPanel.Components;

namespace VetSolutionRation.wpf.Views.RatioPanel.SubPanels.AnimalSelection
{
    internal interface IAnimalSelectionViewModel
    {
    }

    internal sealed class AnimalSelectionViewModel : ViewModelBase, IAnimalSelectionViewModel
    {
        private string? _searchFilter;

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
                    AnimalAdaptersHoster.FilterAnimal(value);
                }
            }
        }
    }
}