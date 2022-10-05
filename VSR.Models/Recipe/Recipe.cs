using System;
using System.Collections.Generic;
using VSR.Enums;
using VSR.Models.Ingredients;

namespace VSR.Models.Recipe;

/// <summary>
/// A Recipe containing a group of ingredient (feed) and a proportion for each
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
    /// List of ingredient for this recipe
    /// </summary>
    IReadOnlyList<IIngredientForRecipe> IngredientsForRecipe { get; }

    // reciped Guid
    Guid Guid { get; }
}

/// <inheritdoc />
public sealed class Recipe : IRecipe
{
    /// <inheritdoc />
    public IReadOnlyList<IIngredientForRecipe> IngredientsForRecipe { get; }

    /// <inheritdoc />
    public Guid Guid { get; }

    /// <inheritdoc />
    public string RecipeName { get; }

    /// <inheritdoc />
    public FeedUnit Unit { get; }

    public Recipe(Guid guid, string recipeName, IReadOnlyList<IIngredientForRecipe> ingredients)
    {
        IngredientsForRecipe = ingredients;
        Guid = guid;
        RecipeName = recipeName;
        Unit = FeedUnit.Kg;
    }
    // public string UniqueReferenceKey => RecipeName; // TODO PBO => clean
}