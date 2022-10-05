using VSR.WPF.Utils.Navigation;

namespace VetSolutionRation.wpf.Views
{
    internal sealed partial class MainPanelView : INavigeablePanel
    {
        public MainPanelView(IMainPanelViewModel vm)
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