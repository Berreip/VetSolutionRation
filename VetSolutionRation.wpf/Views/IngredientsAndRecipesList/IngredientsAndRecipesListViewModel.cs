using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PRF.Utils.CoreComponents.Diagnostic;
using PRF.WPFCore;
using PRF.WPFCore.Commands;
using VetSolutionRation.wpf.Views.Calculation;
using VetSolutionRation.wpf.Views.DuplicatesAndEditFeed;
using VetSolutionRation.wpf.Views.IngredientsAndRecipesList.Adapters;
using VSR.Core.Helpers.Async;
using VSR.Core.Services;
using VSR.WPF.Utils.Adapters.IngredientsAndRecipeList;
using VSR.WPF.Utils.PopupManager;
using VSR.WPF.Utils.Services;

namespace VetSolutionRation.wpf.Views.IngredientsAndRecipesList;

internal interface IIngredientsAndRecipesListViewModel
{
}

internal sealed class IngredientsAndRecipesListViewModel : ViewModelBase, IIngredientsAndRecipesListViewModel
{
    private readonly IIngredientsManager _ingredientsManager;
    private readonly IPopupManagerLight _popupManagerLight;
    private readonly ICalculationViewModel _calculationViewModel;
    private FilterKindAdapter _selectedFilterKind;
    public IIngredientAdaptersHoster IngredientAdaptersHoster { get; }
    
    public IDelegateCommandLight<IngredientForListAdapterBase?> SelectIngredientCommand { get; }
    public IDelegateCommandLight<RecipeForListAdapter?> SelectRecipeCommand { get; }
    public IDelegateCommandLight<IngredientForListAdapterBase> DuplicateFeedCommand { get; }
    
    public IDelegateCommandLight<UserDefinedIngredientForListAdapter> EditIngredientCommand { get; }
    public IDelegateCommandLight<UserDefinedIngredientForListAdapter> DeleteIngredientCommand { get; }
    public IDelegateCommandLight<RecipeForListAdapter?> EditRecipeCommand { get; }
    public IDelegateCommandLight<RecipeForListAdapter?> DuplicateRecipeCommand { get; }
    public IDelegateCommandLight<RecipeForListAdapter?> DeleteRecipeCommand { get; }
    public IReadOnlyList<FilterKindAdapter> AvailableFilterKinds { get; }

    public IngredientsAndRecipesListViewModel(
        ICalculationViewModel calculationViewModel, // TODO PBO => isolate
        IIngredientsManager ingredientsManager,
        IIngredientAdaptersHoster ingredientAdaptersHoster,
        IPopupManagerLight popupManagerLight)
    {
        _calculationViewModel = calculationViewModel;
        _ingredientsManager = ingredientsManager;
        _popupManagerLight = popupManagerLight;
        IngredientAdaptersHoster = ingredientAdaptersHoster;
        AvailableFilterKinds = new[]
        {
            new FilterKindAdapter(FilterKind.All), 
            new FilterKindAdapter(FilterKind.Ingredient), 
            new FilterKindAdapter(FilterKind.Recipe),
        };
        _selectedFilterKind = AvailableFilterKinds[0];
        ingredientAdaptersHoster.SetFilterKind(_selectedFilterKind.Kind);
        
        SelectIngredientCommand = new DelegateCommandLight<IngredientForListAdapterBase?>(ExecuteSelectIngredientCommand);
        SelectRecipeCommand = new DelegateCommandLight<RecipeForListAdapter?>(ExecuteSelectRecipeCommand);
        DuplicateFeedCommand = new DelegateCommandLight<IngredientForListAdapterBase?>(ExecuteDuplicateAndEditIngredientCommand);
        
        EditIngredientCommand = new DelegateCommandLight<UserDefinedIngredientForListAdapter?>(ExecuteEditFeedCommand);
        DeleteIngredientCommand = new DelegateCommandLight<UserDefinedIngredientForListAdapter?>(ExecuteDeleteFeedCommand);
        
        EditRecipeCommand = new DelegateCommandLight<RecipeForListAdapter?>(ExecuteEditRecipeCommand);
        DuplicateRecipeCommand = new DelegateCommandLight<RecipeForListAdapter?>(ExecuteDuplicateRecipeCommand);
        DeleteRecipeCommand = new DelegateCommandLight<RecipeForListAdapter?>(ExecuteDeleteRecipeCommand);
    }

    public FilterKindAdapter SelectedFilterKind
    {
        get => _selectedFilterKind;
        set
        {
            if (SetProperty(ref _selectedFilterKind, value))
            {
                IngredientAdaptersHoster.SetFilterKind(value.Kind);
            }
        }
    }

    private void ExecuteSelectRecipeCommand(RecipeForListAdapter? recipe)
    {  
        if (recipe != null)
        { 
            _calculationViewModel.AddSelectedRecipe(recipe.GetUnderlyingRecipe());
        }
    }

    private void ExecuteSelectIngredientCommand(IngredientForListAdapterBase? ingredient)
    {
        if (ingredient != null)
        { 
            _calculationViewModel.AddSelectedIngredient(ingredient.GetUnderlyingIngredient());
        }
    }

    private void ExecuteEditRecipeCommand(RecipeForListAdapter? recipe)
    {
        AsyncWrapper.Wrap(() =>
        {
            if (recipe == null)
            {
                return;
            }

            DebugCore.Fail("TODO edit à faire");
        });
    }

    private void ExecuteDuplicateRecipeCommand(RecipeForListAdapter? recipe)
    {
        AsyncWrapper.Wrap(() =>
        {
            if (recipe == null)
            {
                return;
            }

            DebugCore.Fail("TODO edit à faire");
        });
    }

    private void ExecuteDeleteRecipeCommand(RecipeForListAdapter? recipe)
    {
        AsyncWrapper.Wrap(() =>
        {
            if (recipe == null)
            {
                return;
            }

            DebugCore.Fail("TODO edit à faire");
        });
    }

    private void ExecuteDeleteFeedCommand(UserDefinedIngredientForListAdapter? ingredient)
    {
        AsyncWrapper.Wrap(() =>
        {
            if (ingredient != null && MessageBox.Show(@$"Voulez vous vraiment supprimer l'aliment {ingredient.Name} ? ", @"Confirmation", MessageBoxButtons.YesNoCancel) == DialogResult.Yes)
            {
                _ingredientsManager.DeleteIngredient(ingredient.GetUnderlyingIngredient());
            }
        });
    }

    private void ExecuteEditFeedCommand(UserDefinedIngredientForListAdapter? feed)
    {
        AsyncWrapper.Wrap(() => ShowEditAndDuplicateWindow(feed, FeedEditionMode.Edition));
    }

    private void ExecuteDuplicateAndEditIngredientCommand(IngredientForListAdapterBase? feed)
    {
        AsyncWrapper.Wrap(() => ShowEditAndDuplicateWindow(feed, FeedEditionMode.Duplication));
    }

    private void ShowEditAndDuplicateWindow(IngredientForListAdapterBase? feed, FeedEditionMode mode)
    {
        if (feed != null)
        {
            _popupManagerLight.ShowDialog(
                () => new DuplicateAndEditFeedPopupViewModel(_popupManagerLight, _ingredientsManager, feed.GetUnderlyingIngredient(), mode),
                vm => new DuplicateAndEditFeedPopupView(vm));
        }
    }
}