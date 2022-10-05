using VSR.Models.Recipe;
using VSR.WPF.Utils.Adapters.Searcheable;

namespace VSR.WPF.Utils.Adapters.IngredientsAndRecipeList;

public sealed class RecipeForListAdapter : SearcheableBase, IIngredientOrRecipeForListAdapter
{
    private readonly IRecipe _recipe;

    public RecipeForListAdapter(IRecipe recipe) : base(recipe.RecipeName)
    {
        _recipe = recipe;
        Name = recipe.RecipeName;
    }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public int DisplayPriority { get; } = DisplayPriorities.RECIPE_PRIORITY;

    public IRecipe GetUnderlyingRecipe() => _recipe;
}