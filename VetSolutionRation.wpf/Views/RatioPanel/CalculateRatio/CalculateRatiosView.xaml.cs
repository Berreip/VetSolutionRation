using VetSolutionRation.wpf.Views.Adapter.FeedSelection;

namespace VetSolutionRation.wpf.Views.RatioPanel.CalculateRatio;

internal sealed partial class CalculateRatiosView : IFeedSelectionModeView
{
    public ICalculateRatiosViewModel ViewModel { get; }
    
    public CalculateRatiosView(ICalculateRatiosViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
        ViewModel = vm;
    }
}