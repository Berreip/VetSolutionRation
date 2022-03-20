using System;
using System.ComponentModel;
using PRF.WPFCore.CustomCollections;
using VetSolutionRatio.wpf.Services.Helpers;
using VetSolutionRatio.wpf.Views.RatioPanel.Adapter;
using VetSolutionRatioLib.Helpers;

namespace VetSolutionRatio.wpf.Views.RatioPanel.Components
{
    /// <summary>
    /// Contains all available animals 
    /// </summary>
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
            AvailableAnimals.Filter = item => SearchFilters.FilterAnimalKind(item, SearchHelpers.SplitByWhitspace(inputText));
        }
    }
}