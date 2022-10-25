using VSR.Models.Ingredients;
using VSR.WPF.Utils.Adapters.Searcheable;
using VSR.WPF.Utils.Helpers;

namespace VSR.WPF.Utils.Adapters.IngredientsAndRecipeList;

/// <summary>
/// Signal an adapter of either a feed or a recipe
/// </summary>
public interface IIngredientOrRecipeForListAdapter : ISearcheable
{
    public string Name { get; }
    public int DisplayPriority { get; }
}

public static class DisplayPriorities
{
    public const int RECIPE_PRIORITY = 30;
    public const int USER_DEFINED_PRIORITY = 20;
    public const int REFERENCE_PRIORITY = 5;
}

/// <summary>
/// Represent the adapter of a feed that has been created by user
/// </summary>
public sealed class UserDefinedIngredientForListAdapter : IngredientForListAdapterBase
{

    public UserDefinedIngredientForListAdapter(IIngredient ingredient) : base(ingredient, DisplayPriorities.USER_DEFINED_PRIORITY)
    {
    }
}

/// <summary>
/// Represent the adapter of an ingredient that is from reference
/// </summary>
public sealed class ReferenceIngredientForListAdapter : IngredientForListAdapterBase
{
    public ReferenceIngredientForListAdapter(IIngredient ingredient) : base(ingredient, DisplayPriorities.REFERENCE_PRIORITY)
    {
    }
}

public abstract class IngredientForListAdapterBase : SearcheableBase, IIngredientOrRecipeForListAdapter
{
    private readonly IIngredient _ingredient;

    protected IngredientForListAdapterBase(IIngredient ingredient, int displayPriority) : base(ingredient.Label)
    {
        _ingredient = ingredient;
        DisplayPriority = displayPriority;
        Name = ingredient.Label;
    }

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public int DisplayPriority { get; }

    public IIngredient GetUnderlyingIngredient() => _ingredient;
}