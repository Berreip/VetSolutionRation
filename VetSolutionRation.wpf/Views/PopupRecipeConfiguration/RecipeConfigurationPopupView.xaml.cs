namespace VetSolutionRation.wpf.Views.RecipeConfiguration;

internal sealed partial class RecipeConfigurationPopupView
{
    public RecipeConfigurationPopupView(RecipeConfigurationPopupViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}