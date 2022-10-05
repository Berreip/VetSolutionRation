using System.ComponentModel;
using System.Linq;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using PRF.WPFCore.CustomCollections;
using VSR.Core.DataProvider;
using VSR.Core.Helpers;
using VSR.WPF.Utils.Adapters;
using VSR.WPF.Utils.Helpers;

namespace VSR.WPF.Utils.Services
{
    /// <summary>
    /// Contains all available animals 
    /// </summary>
    public interface IAnimalAdaptersHoster
    {
        public ICollectionView AvailableAnimals { get; }
        void FilterAnimal(string? inputText);

        /// <summary>
        /// The currently selected animal
        /// </summary>
        AnimalAdapter? SelectedAnimals { get; }

        IDelegateCommandLight<AnimalAdapter> SelectAnimalCommand { get; }
    }

    internal sealed class AnimalAdaptersHoster : ViewModelBase, IAnimalAdaptersHoster
    {
        private AnimalAdapter? _selectedAnimals;
        private readonly ObservableCollectionRanged<AnimalAdapter> _availableAnimals;
        public ICollectionView AvailableAnimals { get; }  
        public IDelegateCommandLight<AnimalAdapter> SelectAnimalCommand { get; }


        public AnimalAdaptersHoster(IAnimalProvider animalProvider)
        {
            var animalAdapters = animalProvider
                .GetAnimals()
                .OrderBy(o => o.Kind)
                .Select(o => new AnimalAdapter(o.Kind, o.Description))
                .ToArray();
         
            AvailableAnimals = ObservableCollectionSource.GetDefaultView(animalAdapters, out _availableAnimals);
            AvailableAnimals.SortDescriptions.Add(new SortDescription(nameof(AnimalAdapter.Kind), ListSortDirection.Ascending));

            SelectAnimalCommand = new DelegateCommandLight<AnimalAdapter>(ExecuteSelectAnimalCommand);
            
            if (animalAdapters.Length > 0)
            {
                ExecuteSelectAnimalCommand(animalAdapters[0]);
            }
        }

        private void ExecuteSelectAnimalCommand(AnimalAdapter animal)
        { 
            foreach (var animalAdapter in _availableAnimals)
            {
                if (ReferenceEquals(animalAdapter, animal))
                {
                    animalAdapter.IsSelected = true;
                    SelectedAnimals = animalAdapter;
                }
                else
                {
                    animalAdapter.IsSelected = false;
                }
            }
        }

        /// <inheritdoc />
        public AnimalAdapter? SelectedAnimals
        {
            get => _selectedAnimals;
            private set => SetProperty(ref _selectedAnimals, value);
        }

        public void FilterAnimal(string? inputText)
        {
            if (inputText == null)
                return;
            var splitByWhitspace = SearchHelpers.SplitByWhitspaceAndSpecificSymbols(inputText);
            AvailableAnimals.Filter = item => SearchFilters.FilterParts(item, splitByWhitspace);
        }
    }
}