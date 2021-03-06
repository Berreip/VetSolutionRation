using VetSolutionRation.wpf.Services.Navigation;

namespace VetSolutionRation.wpf.Views.RatioPanel
{
    internal sealed partial class RatioPanelView : INavigeablePanel
    {
        public RatioPanelView(IRatioPanelViewModel vm)
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