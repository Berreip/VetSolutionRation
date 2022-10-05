using System;
using Newtonsoft.Json;
using VSR.Models.Ingredients;
using VSR.Models.Recipe;

namespace VSR.Dto.Recipe;

public static class IngredientForRecipeExtensions
{
    public static IIngredientForRecipeCandidate ConvertFromDtoToModel(this IngredientForRecipeDto dto)
    {
        if (dto.IngredientGuid == Guid.Empty)
        {
            throw new ArgumentException("no ingredient retrieve from dto");
        }
        return new IngredientForRecipeCandidate(dto.Percentage, dto.IngredientGuid);
    }
    
    public static IngredientForRecipeDto ConvertFromModelToDto(this IIngredientForRecipe model)
    {
        return new IngredientForRecipeDto
        {
            Percentage = model.Percentage,
            IngredientGuid = model.Ingredient.Guid,
        };

    }
}

[JsonObject("IngredientForRecipe")]
public sealed class IngredientForRecipeDto
{
    [JsonProperty("Percentage")]
    public double Percentage { get; set; }
    
    [JsonProperty("Ingredient_Id")]
    public Guid IngredientGuid { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"Ingredient: [{Percentage}% - {IngredientGuid}]";
    }
}