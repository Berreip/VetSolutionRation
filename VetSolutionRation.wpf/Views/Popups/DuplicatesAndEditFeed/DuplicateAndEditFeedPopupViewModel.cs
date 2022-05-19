using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using PRF.WPFCore.CustomCollections;
using VetSolutionRation.wpf.Helpers;
using VetSolutionRation.wpf.Services.Feed;
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
    private readonly IFeedProvider _feedProvider;
    private readonly FeedAdapterBase _currentData;
    private string _feedEditedName;
    private readonly ObservableCollectionRanged<FeedDetailInEditionAdapter> _feedDetailsInEdition;
    private string? _searchFilter;
    private HeaderAdapter? _selectedHeader;
    public ICollectionView FeedDetailsInEdition { get; }
    public IDelegateCommandLight ValidateDuplicateAndEditCommand { get; }
    public ICollectionView AvailableHeaders { get; }
    public IDelegateCommandLight AddCategoryCommand { get; }
    public DelegateCommandLight<FeedDetailInEditionAdapter> DeleteFeedCommand { get; }
    
    public DuplicateAndEditFeedPopupViewModel(IFeedProvider feedProvider, FeedAdapterBase feed)
    {
        _feedProvider = feedProvider;
        _currentData = feed;
        _feedEditedName = $"{_currentData.FeedName}(2)";
        FeedDetailsInEdition = ObservableCollectionSource.GetDefaultView(out _feedDetailsInEdition);
        
        AvailableHeaders = ObservableCollectionSource.GetDefaultView(Enum.GetValues(typeof(InraHeader)).Cast<InraHeader>().Select(o => new HeaderAdapter(o)));
        AvailableHeaders.SortDescriptions.Add(new SortDescription(nameof(HeaderAdapter.Header), ListSortDirection.Ascending));
        
        ValidateDuplicateAndEditCommand = new DelegateCommandLight(ExecuteValidateDuplicateAndEditCommand, CanExecuteValidateDuplicateAndEditCommand);
        AddCategoryCommand = new DelegateCommandLight(ExecuteAddCategoryCommand, CanExecuteAddCategoryCommand);
        DeleteFeedCommand = new DelegateCommandLight<FeedDetailInEditionAdapter>(ExecuteDeleteFeedCommand);
        
        // LoadDefaultHeader:
        _feedDetailsInEdition.AddRange(FeedInEditionHelpers.GetDefaultHeaderForEdition(feed, RefreshValidity));
        RefreshAvailableFeeds();
    }

    private void ExecuteDeleteFeedCommand(FeedDetailInEditionAdapter feedDetailsToRemove)
    {
        if (_feedDetailsInEdition.Remove(feedDetailsToRemove))
        {
            RefreshValidity();
        }
    }

    private bool CanExecuteAddCategoryCommand()
    {
        return _selectedHeader != null;
    }

    private void ExecuteAddCategoryCommand()
    {
        var header = _selectedHeader;
        if (header != null)
        {
            _feedDetailsInEdition.Add(new FeedDetailInEditionAdapter(header.HeaderKind, _currentData.GetInraValue(header.HeaderKind), RefreshValidity));
            RefreshAvailableFeeds();
        }
    }

    private void RefreshAvailableFeeds()
    {
        var presentFeeds = _feedDetailsInEdition.Select(o => o.Header).ToHashSet();
        AvailableHeaders.Filter = item => FeedInEditionHelpers.FilterAvailableHeaders(item, presentFeeds);
        RefreshValidity();
    }

    private bool CanExecuteValidateDuplicateAndEditCommand()
    {
        return !string.IsNullOrWhiteSpace(_feedEditedName) &&
               _feedDetailsInEdition.Count != 0 && 
               _feedDetailsInEdition.All(o => o.IsValid);
    }

    private void ExecuteValidateDuplicateAndEditCommand()
    {
        var customFeed = new CustomFeed(new[] { _feedEditedName }, _feedDetailsInEdition.Select(o => o.CreateNutritionalFeedDetails()));
        _feedProvider.AddFeedsAndSave(new []{customFeed});
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

    public HeaderAdapter? SelectedHeader
    {
        get => _selectedHeader;
        set
        {
            if (SetProperty(ref _selectedHeader, value))
            {
                AddCategoryCommand.RaiseCanExecuteChanged();
            }
        }
    }

    private void RefreshValidity()
    {
        ValidateDuplicateAndEditCommand.RaiseCanExecuteChanged();
    }
}