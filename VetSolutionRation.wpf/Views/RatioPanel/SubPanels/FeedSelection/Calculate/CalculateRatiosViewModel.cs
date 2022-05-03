using PRF.WPFCore;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Adapters;

namespace VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Calculate;

internal interface ICalculateRatiosViewModel
{
    void AddSelectedFeed(ReferenceFeedAdapter feedAdapter);
}

internal sealed class CalculateRatiosViewModel : ViewModelBase, ICalculateRatiosViewModel
{
    /// <inheritdoc />
    public void AddSelectedFeed(ReferenceFeedAdapter feedAdapter)
    {
        // Nothing now
    }
}