using System.ComponentModel;
using System.Linq;
using PRF.WPFCore.CustomCollections;
using VetSolutionRation.wpf.Helpers;
using VetSolutionRation.wpf.Views.RatioPanel.Adapter;
using VetSolutionRationLib.DataProvider;
using VetSolutionRationLib.Helpers;

namespace VetSolutionRation.wpf.Views.RatioPanel.Components
{
    /// <summary>
    /// Contains all available animals 
    /// </summary>
    internal interface IAnimalAdaptersHoster
    {
        public ICollectionView AvailableAnimals { get; }
        void FilterAnimal(string? inputText);
    }

    internal sealed class AnimalAdaptersHoster : IAnimalAdaptersHoster
    {
        public ICollectionView AvailableAnimals { get; }

        public AnimalAdaptersHoster(IAnimalProvider animalProvider)
        {
            AvailableAnimals = ObservableCollectionSource.GetDefaultView(animalProvider.GetAnimals().Select(o => new AnimalAdapter(o.Kind, o.SubKind, "toto toutou tata")));
        }
        
        public void FilterAnimal(string? inputText)
        {
            if (inputText == null)
                return;
            var splitByWhitspace = SearchHelpers.SplitByWhitspace(inputText);
            AvailableAnimals.Filter = item => SearchFilters.FilterParts(item, splitByWhitspace);
        }
    }
}