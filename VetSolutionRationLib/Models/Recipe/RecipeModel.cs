using System.Collections.Generic;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models.Feed;

namespace VetSolutionRationLib.Models.Recipe;

/// <summary>
/// A Recipe containing a group of ingredient (feed) and a proportion for each
/// </summary>
public interface IRecipe : IFeedOrReciepe
{
    /// <summary>
    /// The name of the recipe
    /// </summary>
    string RecipeName { get; }

    /// <summary>
    ///  the unit for displayed
    /// </summary>
    FeedUnit Unit { get; }
    
    /// <summary>
    /// List of ingredient for this recipe
    /// </summary>
    IReadOnlyList<IIngredientForRecipe> Ingredients { get; }
}

/// <summary>
/// An ingredient which is a part of a recipe
/// </summary>
public interface IIngredientForRecipe
{
    /// <summary>
    /// The percentage of this ingredient in the recipe total
    /// </summary>
    double Percentage { get; }
    
    /// <summary>
    /// the related ingredient
    /// </summary>
    IFeed Ingredient { get; }
}

/// <inheritdoc />
public sealed class IngredientForRecipe : IIngredientForRecipe
{
    /// <inheritdoc />
    public double Percentage { get; }

    /// <inheritdoc />
    public IFeed Ingredient { get; }
    
    public IngredientForRecipe(double percentage, IFeed feed)
    {
        Percentage = percentage;
        Ingredient = feed;
    }
}

/// <inheritdoc />
public sealed class RecipeModel : IRecipe
{
    /// <inheritdoc />
    public IReadOnlyList<IIngredientForRecipe> Ingredients { get; }

    /// <inheritdoc />
    public string RecipeName { get; }

    /// <inheritdoc />
    public FeedUnit Unit { get; }

    public RecipeModel(string recipeName, FeedUnit unit,  IReadOnlyList<IIngredientForRecipe> ingredients)
    {
        Ingredients = ingredients;
        RecipeName = recipeName;
        Unit = unit;
    }
}