namespace VSR.WPF.Utils.Assertions
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
