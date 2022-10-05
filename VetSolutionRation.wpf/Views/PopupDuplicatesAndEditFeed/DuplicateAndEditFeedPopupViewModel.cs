using System;
using System.ComponentModel;
using System.Linq;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using PRF.WPFCore.CustomCollections;
using VSR.Core.Services;
using VSR.Enums;
using VSR.Models.Ingredients;
using VSR.WPF.Utils.Adapters;
using VSR.WPF.Utils.Adapters.EditionIngredients;
using VSR.WPF.Utils.PopupManager;

namespace VetSolutionRation.wpf.Views.DuplicatesAndEditFeed;

internal interface IDuplicateAndEditFeedPopupViewModel: IPopupViewModel
{
}

internal sealed class DuplicateAndEditFeedPopupViewModel : ViewModelBase, IDuplicateAndEditFeedPopupViewModel
{
    private readonly IPopupManagerLight _popupManager;
    private readonly IIngredientsManager _ingredientsManager;
    private readonly IIngredient _currentData;
    private readonly FeedEditionMode _mode;
    private string _feedEditedName;
    private string? _searchFilter;
    private InraHeaderAdapter? _selectedHeader;
    private bool _isDuplicatedLabel;
    public bool CouldEditName { get; }
    
    private readonly ObservableCollectionRanged<NutritionalDetailsAdapter> _ingredientDetailsInEdition;
    public ICollectionView IngredientDetailsInEdition { get; }
    public ICollectionView AvailableHeaders { get; }
    
    public IDelegateCommandLight ValidateDuplicateAndEditCommand { get; }
    public IDelegateCommandLight AddCategoryCommand { get; }
    public DelegateCommandLight<NutritionalDetailsAdapter> DeleteFeedCommand { get; }

    public DuplicateAndEditFeedPopupViewModel(IPopupManagerLight popupManager, IIngredientsManager ingredientsManager, IIngredient feed, FeedEditionMode mode)
    {
        _popupManager = popupManager;
        _ingredientsManager = ingredientsManager;
        _currentData = feed;
        _mode = mode;
        _feedEditedName = feed.Label;

        if (mode == FeedEditionMode.Edition && !feed.IsUserAdded)
        {
            throw new InvalidOperationException("Could not edit a reference feed");
        }

        CouldEditName = _mode == FeedEditionMode.Duplication;
        IngredientDetailsInEdition = ObservableCollectionSource.GetDefaultView(out _ingredientDetailsInEdition);
        AvailableHeaders = ObservableCollectionSource.GetDefaultView(Enum.GetValues(typeof(InraHeader)).Cast<InraHeader>().Select(o => new InraHeaderAdapter(o)));
        AvailableHeaders.SortDescriptions.Add(new SortDescription(nameof(InraHeaderAdapter.Header), ListSortDirection.Ascending));

        ValidateDuplicateAndEditCommand = new DelegateCommandLight(ExecuteValidateDuplicateAndEditCommand, CanExecuteValidateDuplicateAndEditCommand);
        AddCategoryCommand = new DelegateCommandLight(ExecuteAddCategoryCommand, CanExecuteAddCategoryCommand);
        DeleteFeedCommand = new DelegateCommandLight<NutritionalDetailsAdapter>(ExecuteDeleteFeedCommand);

        // LoadDefaultHeader:
        _ingredientDetailsInEdition.AddRange(FeedInEditionHelpers.GetDefaultHeaderForEdition(feed, RefreshValidity));
        RefreshAvailableFeeds();
    }

    private void ExecuteDeleteFeedCommand(NutritionalDetailsAdapter feedDetailsToRemove)
    {
        if (_ingredientDetailsInEdition.Remove(feedDetailsToRemove))
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
            var detailsValue = _currentData.TryGetNutritionDetail(header.HeaderKind, out var nutritionalDetails) ? nutritionalDetails.Value : 0;
            _ingredientDetailsInEdition.Add(new NutritionalDetailsAdapter(header.HeaderKind, detailsValue, RefreshValidity));
            SelectedHeader = null;
            RefreshAvailableFeeds();
        }
    }

    private void RefreshAvailableFeeds()
    {
        var presentFeeds = _ingredientDetailsInEdition.Select(o => o.Header).ToHashSet();
        AvailableHeaders.Filter = item => FeedInEditionHelpers.FilterAvailableHeaders(item, presentFeeds);
        RefreshValidity();
    }

    private bool CanExecuteValidateDuplicateAndEditCommand()
    {
        return !string.IsNullOrWhiteSpace(_feedEditedName) &&
               !_isDuplicatedLabel &&
               _ingredientDetailsInEdition.Count != 0 &&
               _ingredientDetailsInEdition.All(o => o.IsValid);
    }

    private void ExecuteValidateDuplicateAndEditCommand()
    {
        var newIngredient = new Ingredient(Guid.NewGuid(), _feedEditedName, true, _ingredientDetailsInEdition.Select(o => o.CreateNutritionalFeedDetails()));
        _ingredientsManager.AddIngredients(new[] { newIngredient });
        
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
                    IngredientDetailsInEdition.Filter = null;
                }
                else
                {
                    IngredientDetailsInEdition.Filter = item => FeedInEditionHelpers.FilterDetailsParts(item, value);
                }
            }
        }
    }

    public InraHeaderAdapter? SelectedHeader
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
        IsDuplicatedLabel = CouldEditName && _ingredientsManager.TryGetIngredientByName(_feedEditedName, out _);
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
    Duplication,
}