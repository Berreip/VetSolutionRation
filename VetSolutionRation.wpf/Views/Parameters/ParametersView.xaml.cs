using VetSolutionRation.wpf.Services.Navigation;

namespace VetSolutionRation.wpf.Views.Parameters
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