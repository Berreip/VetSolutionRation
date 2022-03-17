using VetSolutionRatio.wpf.Services.Navigation;

namespace VetSolutionRatio.wpf.Views.Parameters
{
    internal sealed partial class ParametersView : INavigeablePanel
    {
        public ParametersView(IParametersViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }

        public void OnNavigateTo()
        {
            // do nothing
        }

        public void OnNavigateExit()
        {
            // do nothing
        }
    }
}