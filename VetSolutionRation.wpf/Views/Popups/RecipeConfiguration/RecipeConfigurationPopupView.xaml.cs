namespace VetSolutionRation.wpf.Views.Popups.RecipeConfiguration;

internal sealed partial class RecipeConfigurationPopupView
{
    public RecipeConfigurationPopupView(RecipeConfigurationPopupViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}