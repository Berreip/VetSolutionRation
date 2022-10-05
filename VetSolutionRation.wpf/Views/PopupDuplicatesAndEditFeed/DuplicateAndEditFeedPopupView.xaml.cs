namespace VetSolutionRation.wpf.Views.DuplicatesAndEditFeed;

internal sealed partial class DuplicateAndEditFeedPopupView
{
    public DuplicateAndEditFeedPopupView(IDuplicateAndEditFeedPopupViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}