namespace VSR.Models.Ingredients;

/// <summary>
/// An ingredient which is a part of a recipe and so, has a percentage repartition
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
    IIngredient Ingredient { get; }
}

/// <inheritdoc />
public sealed class IngredientForRecipe : IIngredientForRecipe
{
    /// <inheritdoc />
    public double Percentage { get; }

    /// <inheritdoc />
    public IIngredient Ingredient { get; }
    
    public IngredientForRecipe(double percentage, IIngredient feed)
    {
        Percentage = percentage;
        Ingredient = feed;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"{Ingredient.Label} ({Ingredient.Guid}) = {Percentage}%";
    }
}