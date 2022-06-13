using System.Collections.Generic;
using System.Diagnostics;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models.Feed;

namespace VetSolutionRationLib.Models.Recipe;

/// <summary>
/// A group of feed in a certains proportion
/// </summary>
public interface IRecipe
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
    /// The quantity
    /// </summary>
    double Quantity { get; }
}

/// <summary>
/// An ingredient which is a part of a recipe
/// </summary>
public interface IFeedForRecipe
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

public sealed class RecipeModel : IRecipe
{
    private readonly IReadOnlyList<IFeedForRecipe> _ingredients;

    /// <inheritdoc />
    public string RecipeName { get; }

    /// <inheritdoc />
    public FeedUnit Unit { get; }

    /// <inheritdoc />
    public double Quantity { get; }

    public RecipeModel(string recipeName, FeedUnit unit,  IReadOnlyList<IFeedForRecipe> ingredients)
    {
        _ingredients = ingredients;
        RecipeName = recipeName;
        Unit = unit;
        Quantity = 0;
    }
}