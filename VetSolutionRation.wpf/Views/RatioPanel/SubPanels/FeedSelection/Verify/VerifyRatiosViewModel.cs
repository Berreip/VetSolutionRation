using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Xps.Packaging;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using PRF.WPFCore.CustomCollections;
using PRF.WPFCore.LiveCollection;
using VetSolutionRation.wpf.Services;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Adapters;

namespace VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Verify;

internal interface IVerifyRatiosViewModel
{
    void AddSelectedFeed(FeedAdapter feedAdapter);
}

internal sealed class VerifyRatiosViewModel : ViewModelBase, IVerifyRatiosViewModel
{
    public ICollectionView SelectedFeedsForVerifyPanel { get; }
    private readonly ObservableCollectionRanged<FeedVerifySpecificAdapter> _selectedFeedForVerifyPanel;
    private readonly HashSet<FeedAdapter> _alreadyAddedHash = new HashSet<FeedAdapter>();

    public IDelegateCommandLight<FeedVerifySpecificAdapter> RemoveFromSelectedFeedsCommand { get; }
    
    /// <inheritdoc />
    public VerifyRatiosViewModel()
    {
        SelectedFeedsForVerifyPanel = ObservableCollectionSource.GetDefaultView(out _selectedFeedForVerifyPanel);
        RemoveFromSelectedFeedsCommand = new DelegateCommandLight<FeedVerifySpecificAdapter>(ExecuteRemoveFromSelectedFeedsCommand);
    }

    /// <inheritdoc />
    public void AddSelectedFeed(FeedAdapter feedAdapter)
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