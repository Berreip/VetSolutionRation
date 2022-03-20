using System;
using System.ComponentModel;
using System.Linq;
using PRF.WPFCore.CustomCollections;
using VetSolutionRatio.wpf.Services.Helpers;
using VetSolutionRatio.wpf.Views.RatioPanel.Adapter;
using VetSolutionRatioLib.DataProvider;
using VetSolutionRatioLib.Helpers;

namespace VetSolutionRatio.wpf.Views.RatioPanel.Components
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
        private ObservableCollectionRanged<AnimalAdapter> _availableAnimalKind;
        public ICollectionView AvailableAnimals { get; }

        public AnimalAdaptersHoster(IAnimalProvider animalProvider)
        {
            AvailableAnimals = ObservableCollectionSource.GetDefaultView(animalProvider.GetAnimals().Select(o => new AnimalAdapter(o.Kind, o.SubKind, "toto toutou tata")), out _availableAnimalKind);
        }
        
        public void FilterAnimal(string? inputText)
        {
            if (inputText == null)
                return;
            AvailableAnimals.Filter = item => SearchFilters.FilterAnimalKind(item, SearchHelpers.SplitByWhitspace(inputText));
        }
    }
}