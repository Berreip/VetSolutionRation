namespace VetSolutionRation.wpf.Views.Popups.DuplicatesAndEditFeed;

internal sealed partial class DuplicateAndEditFeedPopupView
{
    public DuplicateAndEditFeedPopupView(IDuplicateAndEditFeedPopupViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}