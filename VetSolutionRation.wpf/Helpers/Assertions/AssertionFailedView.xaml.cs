namespace VetSolutionRation.wpf.Helpers.Assertions
{
    internal sealed partial class AssertionFailedView
    {
        public AssertionFailedView(AssertionFailedViewModel dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
        }
    }
}
