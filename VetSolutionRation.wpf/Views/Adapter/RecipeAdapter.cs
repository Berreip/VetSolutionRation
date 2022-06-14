using PRF.WPFCore;
using VetSolutionRation.wpf.Views.RatioPanel.Recipe;
using VetSolutionRationLib.Models.Recipe;

namespace VetSolutionRation.wpf.Views.Adapter;

internal interface IRecipeAdapter : IFeedThatCouldBeAddedIntoReciepe, IFeedOrReciepe
{
}

internal sealed class RecipeAdapter : ViewModelBase, IRecipeAdapter
{
    private bool _isSelected = true;

    public RecipeAdapter(IRecipe recipe)
    {
        Name = recipe.RecipeName;
        FeedQuantity = new FeedQuantityAdapter(recipe.Unit, recipe.Quantity);
    }

    /// <inheritdoc />
    public bool IsSelected
    {
        get => _isSelected;
        set => SetProperty(ref _isSelected, value);
    }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public IFeedQuantityAdapter FeedQuantity { get; }

}