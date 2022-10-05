using PRF.WPFCore;
using VetSolutionRation.wpf.Views.AnimalSelection;
using VetSolutionRation.wpf.Views.Calculation;
using VetSolutionRation.wpf.Views.IngredientsAndRecipesList;

namespace VetSolutionRation.wpf.Views
{
    internal interface IMainPanelViewModel
    {
    }

    internal sealed class MainPanelViewModel : ViewModelBase, IMainPanelViewModel
    {
        public MainPanelViewModel(
            AnimalSelectionView animalSelectionView,
            IngredientsAndRecipesListView ingredientsAndRecipesListView, 
            CalculationView calculationView)
        {
            AnimalSelectionView = animalSelectionView;
            IngredientsAndRecipesListView = ingredientsAndRecipesListView;
            CalculationView = calculationView;
        }

        public AnimalSelectionView AnimalSelectionView { get; }
        public IngredientsAndRecipesListView IngredientsAndRecipesListView { get; }
        public CalculationView CalculationView { get; }
    }
}