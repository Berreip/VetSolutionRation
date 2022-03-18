using System;
using System.ComponentModel;
using PRF.WPFCore.CustomCollections;
using VetSolutionRatio.wpf.Views.RatioPanel.Adapter;
using VetSolutionRatioLib.Helpers;

namespace VetSolutionRatio.wpf.Views.RatioPanel.Components
{
    internal interface IAnimalKindHoster
    {
        public ICollectionView AvailableAnimals { get; }
        void FilterAnimal(string? inputText);
    }

    internal sealed class AnimalKindHoster : IAnimalKindHoster
    {
        private ObservableCollectionRanged<AnimalKindAdapter> _availableAnimalKind;
        public ICollectionView AvailableAnimals { get; }

        public AnimalKindHoster()
        {
            AvailableAnimals = ObservableCollectionSource.GetDefaultView(out _availableAnimalKind);
        }
        
        public void FilterAnimal(string? inputText)
        {
            if (inputText == null)
                return;
            AvailableAnimals.Filter = item => FilterAnimalKind(item, SearchHelpers.SplitByWhitspace(inputText));
        }
        
        private static bool FilterAnimalKind(object obj, string[] searchText)
        {
            if (obj is AnimalKindAdapter animal)
            {
                return animal.ContainsAll(searchText);
            }
            return false;
        }
    }
}