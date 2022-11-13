namespace VetSolutionRation.wpf.Views.PopupRecipeConfiguration;

internal sealed partial class RecipeConfigurationPopupView
{
    public RecipeConfigurationPopupView(RecipeConfigurationPopupViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}