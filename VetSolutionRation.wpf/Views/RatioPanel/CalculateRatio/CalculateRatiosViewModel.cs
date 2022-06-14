using PRF.WPFCore;
using VetSolutionRation.wpf.Views.Adapter;

namespace VetSolutionRation.wpf.Views.RatioPanel.CalculateRatio;

internal interface ICalculateRatiosViewModel
{
    void AddSelectedFeed(IFeedAdapter feedAdapter);
}

internal sealed class CalculateRatiosViewModel : ViewModelBase, ICalculateRatiosViewModel
{
    /// <inheritdoc />
    public void AddSelectedFeed(IFeedAdapter feedAdapter)
    {
        // Nothing now
    }
}