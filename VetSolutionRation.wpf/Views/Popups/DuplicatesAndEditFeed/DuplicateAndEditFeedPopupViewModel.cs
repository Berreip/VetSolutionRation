using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using PRF.WPFCore.CustomCollections;
using VetSolutionRation.wpf.Helpers;
using VetSolutionRation.wpf.Views.Popups.Adapters;
using VetSolutionRation.wpf.Views.RatioPanel.SubPanels.FeedSelection.Adapters;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models.Feed;

namespace VetSolutionRation.wpf.Views.Popups.DuplicatesAndEditFeed;

internal interface IDuplicateAndEditFeedPopupViewModel
{
}

internal sealed class DuplicateAndEditFeedPopupViewModel : ViewModelBase, IDuplicateAndEditFeedPopupViewModel
{
    private readonly FeedAdapterBase _currentData;
    private readonly Action<IFeed> _onDuplicateFeedValidated;
    private string _feedEditedName;
    private ObservableCollectionRanged<FeedDetailInEditionAdapter> _feedDetailsInEdition;
    private string? _searchFilter;
    public ICollectionView FeedDetailsInEdition { get; }
    public IDelegateCommandLight ValidateDuplicateAndEditCommand { get; }
    public ICollectionView AvailableHeaders { get; }

    public DuplicateAndEditFeedPopupViewModel(FeedAdapterBase feed, Action<IFeed> onDuplicateFeedValidated)
    {
        _currentData = feed;
        _onDuplicateFeedValidated = onDuplicateFeedValidated;
        _feedEditedName = $"{_currentData.FeedName}(2)";
        FeedDetailsInEdition = ObservableCollectionSource.GetDefaultView(out _feedDetailsInEdition);
        
        AvailableHeaders = ObservableCollectionSource.GetDefaultView(Enum.GetValues(typeof(InraHeader)).Cast<InraHeader>().Select(o => new HeaderAdapter(o)));
        AvailableHeaders.SortDescriptions.Add(new SortDescription(nameof(HeaderAdapter.Header), ListSortDirection.Ascending));
        
        ValidateDuplicateAndEditCommand = new DelegateCommandLight(ExecuteValidateDuplicateAndEditCommand, CanExecuteValidateDuplicateAndEditCommand);
        LoadDefaultHeader();
    }

    private void LoadDefaultHeader()
    {
        _feedDetailsInEdition.AddRange(FeedInEditionHelpers.GetDefaultHeaderForEdition());
        var availableFeed = _feedDetailsInEdition.Select(o => o.Header).ToHashSet();
        AvailableHeaders.Filter =  item => FeedInEditionHelpers.FilterAvailableHeaders(item, availableFeed);
    }

    private bool CanExecuteValidateDuplicateAndEditCommand()
    {
        return !string.IsNullOrWhiteSpace(_feedEditedName);
    }

    private void ExecuteValidateDuplicateAndEditCommand()
    {
        // TODO PBO 
        _onDuplicateFeedValidated.Invoke(new CustomFeed(new[] { _feedEditedName }, new List<INutritionalFeedDetails>(), new List<IStringDetailsContent>()));
    }

    public string FeedEditedName
    {
        get => _feedEditedName;
        set
        {
            if (SetProperty(ref _feedEditedName, value))
            {
                RefreshValidity();
            }
        }
    }

    public string? SearchFilter
    {
        get => _searchFilter;
        set
        {
            if (SetProperty(ref _searchFilter, value))
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    FeedDetailsInEdition.Filter = null;
                }
                else
                {
                    FeedDetailsInEdition.Filter = item => FeedInEditionHelpers.FilterDetailsParts(item, value);
                }
            }
        }
    }

    private void RefreshValidity()
    {
        ValidateDuplicateAndEditCommand.RaiseCanExecuteChanged();
    }
}