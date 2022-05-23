using System.Collections.Generic;
using System.ComponentModel;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using PRF.WPFCore.CustomCollections;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Adapters;

namespace VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Verify;

internal interface IVerifyRatiosViewModel
{
    void AddSelectedFeed(IFeedAdapter feedAdapter);
}

internal sealed class VerifyRatiosViewModel : ViewModelBase, IVerifyRatiosViewModel
{
    public ICollectionView SelectedFeedsForVerifyPanel { get; }
    private readonly ObservableCollectionRanged<FeedVerifySpecificAdapter> _selectedFeedForVerifyPanel;
    private readonly HashSet<IFeedAdapter> _alreadyAddedHash = new HashSet<IFeedAdapter>();

    public IDelegateCommandLight<FeedVerifySpecificAdapter> RemoveFromSelectedFeedsCommand { get; }
    
    /// <inheritdoc />
    public VerifyRatiosViewModel()
    {
        SelectedFeedsForVerifyPanel = ObservableCollectionSource.GetDefaultView(out _selectedFeedForVerifyPanel);
        RemoveFromSelectedFeedsCommand = new DelegateCommandLight<FeedVerifySpecificAdapter>(ExecuteRemoveFromSelectedFeedsCommand);
    }

    /// <inheritdoc />
    public void AddSelectedFeed(IFeedAdapter feedAdapter)
    {
        if (_alreadyAddedHash.Add(feedAdapter))
        {
            _selectedFeedForVerifyPanel.Add(new FeedVerifySpecificAdapter(feedAdapter, "kg", true));
        }
    }
    
    private void ExecuteRemoveFromSelectedFeedsCommand(FeedVerifySpecificAdapter feed)
    {
        if(_alreadyAddedHash.Remove(feed.GetUnderlyingFeedAdapter()))
        {
            _selectedFeedForVerifyPanel.Remove(feed);
        }
    }
}