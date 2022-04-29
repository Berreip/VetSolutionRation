using PRF.WPFCore;
using VetSolutionRation.wpf.Searcheable;

namespace VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Adapters;

internal interface IFeedAdapter
{
}

internal sealed class FeedAdapter : SearcheableBase, IFeedAdapter
{
    private bool _isSelected;

    public FeedAdapter(string feedName) : base(feedName)
    {
        FeedName = feedName;
    }
    
    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    public string FeedName { get; }
}
