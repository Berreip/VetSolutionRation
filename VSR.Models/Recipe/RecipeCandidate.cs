using System;
using System.Collections.Generic;

namespace VSR.Models.Recipe;

/// <summary>
/// When a recipe is extracted from file, it is not sure that every ingredients is still available in the current software datas. To
/// avoid keeping the same ingredient in the ingredient list AND in the recipe (with data consolidation associated with) we keep only the
/// Ingredient Guid in recipe and consolidate later
/// </summary>
public interface IRecipeCandidate
{
    public string Name { get; }
    public IReadOnlyList<IIngredientForRecipeCandidate> Ingredients { get; }
    Guid Guid { get; }
}

/// <summary>
/// When a recipe is extracted from file, it is not sure that every ingredients is still available in the current software datas. To
/// avoid keeping the same ingredient in the ingredient list AND in the recipe (with data consolidation associated with) we keep only the
/// Ingredient Guid in recipe and consolidate later
/// </summary>
public interface IIngredientForRecipeCandidate
{
    double Percentage { get; }
    Guid IngredientGuid { get; }
}

/// <inheritdoc />
public sealed class RecipeCandidate : IRecipeCandidate
{
    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public IReadOnlyList<IIngredientForRecipeCandidate> Ingredients { get; }

    /// <inheritdoc />
    public Guid Guid { get; }

    public RecipeCandidate(Guid guid, string name, IReadOnlyList<IIngredientForRecipeCandidate> ingredients)
    {
        Guid = guid;
        Name = name;
        Ingredients = ingredients;
    }
}

public sealed class IngredientForRecipeCandidate : IIngredientForRecipeCandidate
{
    public double Percentage { get; }
    public Guid IngredientGuid { get; }

    public IngredientForRecipeCandidate(double percentage, Guid ingredientGuid)
    {
        Percentage = percentage;
        IngredientGuid = ingredientGuid;
    }
}