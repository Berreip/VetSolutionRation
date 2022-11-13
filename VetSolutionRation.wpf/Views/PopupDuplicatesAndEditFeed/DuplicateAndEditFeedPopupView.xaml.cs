namespace VetSolutionRation.wpf.Views.PopupDuplicatesAndEditFeed;

internal sealed partial class DuplicateAndEditFeedPopupView
{
    public DuplicateAndEditFeedPopupView(IDuplicateAndEditFeedPopupViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }
}