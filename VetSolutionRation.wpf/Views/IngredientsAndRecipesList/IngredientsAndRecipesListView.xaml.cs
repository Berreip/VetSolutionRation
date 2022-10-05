namespace VetSolutionRation.wpf.Views.IngredientsAndRecipesList;

internal sealed partial class IngredientsAndRecipesListView
{
    public IngredientsAndRecipesListView(IIngredientsAndRecipesListViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}