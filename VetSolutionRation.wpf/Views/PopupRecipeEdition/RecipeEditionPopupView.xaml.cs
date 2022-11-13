using System.Windows;

namespace VetSolutionRation.wpf.Views.PopupRecipeEdition;

internal sealed partial class RecipeEditionPopupView 
{
   
    public RecipeEditionPopupView(RecipeEditionPopupViewModel dataContext)
    {
        InitializeComponent();
        DataContext = dataContext;
    }
}