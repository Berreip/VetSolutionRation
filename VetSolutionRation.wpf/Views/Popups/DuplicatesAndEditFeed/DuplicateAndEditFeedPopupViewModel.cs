using System;
using System.ComponentModel;
using System.Linq;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using PRF.WPFCore.CustomCollections;
using VetSolutionRation.wpf.Services.PopupManager;
using VetSolutionRation.wpf.Services.Saves;
using VetSolutionRation.wpf.Views.Adapter;
using VetSolutionRation.wpf.Views.Adapter.Feeds;
using VetSolutionRation.wpf.Views.Popups.Adapters;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models.Feed;

namespace VetSolutionRation.wpf.Views.Popups.DuplicatesAndEditFeed;

internal interface IDuplicateAndEditFeedPopupViewModel: IPopupViewModel
{
}

internal sealed class DuplicateAndEditFeedPopupViewModel : ViewModelBase, IDuplicateAndEditFeedPopupViewModel
{
    private readonly IPopupManagerLight _popupManager;
    private readonly IFeedProvider _feedProvider;
    private readonly IFeedAdapter _currentData;
    private readonly FeedEditionMode _mode;
    private string _feedEditedName;
    private string? _searchFilter;
    private HeaderAdapter? _selectedHeader;
    private bool _isDuplicatedLabel;
    public bool CouldEditName { get; }
    
    private readonly ObservableCollectionRanged<FeedDetailInEditionAdapter> _feedDetailsInEdition;
    public ICollectionView FeedDetailsInEdition { get; }
    public ICollectionView AvailableHeaders { get; }
    
    public IDelegateCommandLight ValidateDuplicateAndEditCommand { get; }
    public IDelegateCommandLight AddCategoryCommand { get; }
    public DelegateCommandLight<FeedDetailInEditionAdapter> DeleteFeedCommand { get; }

    public DuplicateAndEditFeedPopupViewModel(IPopupManagerLight popupManager, IFeedProvider feedProvider, IFeedAdapter feed, FeedEditionMode mode)
    {
        _popupManager = popupManager;
        _feedProvider = feedProvider;
        _currentData = feed;
        _mode = mode;
        _feedEditedName = feed.Name;

        if (mode == FeedEditionMode.Edition && feed is IReferenceFeedAdapter)
        {
            throw new InvalidOperationException("Could not edit a reference feed");
        }

        CouldEditName = _mode == FeedEditionMode.Duplication;
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
            RefreshAvailableFeeds();
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
            SelectedHeader = null;
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
               !_isDuplicatedLabel &&
               _feedDetailsInEdition.Count != 0 &&
               _feedDetailsInEdition.All(o => o.IsValid);
    }

    private void ExecuteValidateDuplicateAndEditCommand()
    {
        var customFeed = new CustomFeed(new[] { _feedEditedName }, _feedDetailsInEdition.Select(o => o.CreateNutritionalFeedDetails()), Guid.NewGuid());
        _feedProvider.AddFeedsAndSave(new[] { customFeed });
        
        // request closing
        _popupManager.RequestClosing(this);
    }
    
    public string FeedEditedName
    {
        get => _feedEditedName;
        set
        {
            if (_mode == FeedEditionMode.Duplication && SetProperty(ref _feedEditedName, value))
            {
                RefreshValidity();
            }
        }
    }

    public bool IsDuplicatedLabel
    {
        get => _isDuplicatedLabel;
        private set => SetProperty(ref _isDuplicatedLabel, value);
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
        IsDuplicatedLabel = CouldEditName && _feedProvider.ContainsName(_feedEditedName);
        ValidateDuplicateAndEditCommand.RaiseCanExecuteChanged();
    }
}

internal enum FeedEditionMode
{
    /// <summary>
    /// Edit a feed without changing it's name
    /// </summary>
    Edition,

    /// <summary>
    /// Duplicate a feed
    /// </summary>
    Duplication
}