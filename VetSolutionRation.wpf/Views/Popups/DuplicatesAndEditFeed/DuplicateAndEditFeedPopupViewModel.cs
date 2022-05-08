using System;
using PRF.WPFCore;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Adapters;
using VetSolutionRationLib.Models.Feed;

namespace VetSolutionRation.wpf.Views.Popups.DuplicatesAndEditFeed;

internal interface IDuplicateAndEditFeedPopupViewModel
{
}

internal sealed class DuplicateAndEditFeedPopupViewModel: ViewModelBase, IDuplicateAndEditFeedPopupViewModel
{
    private readonly FeedAdapterBase _currentData;
    private readonly Action<IFeed> _onDuplicateFeedValidated;

    public DuplicateAndEditFeedPopupViewModel(FeedAdapterBase feed, Action<IFeed> onDuplicateFeedValidated)
    {
        _currentData = feed;
        _onDuplicateFeedValidated = onDuplicateFeedValidated;
    }
}