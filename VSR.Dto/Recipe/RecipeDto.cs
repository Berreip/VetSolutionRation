using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using VSR.Dto.Ingredients;
using VSR.Models.Recipe;

namespace VSR.Dto.Recipe;

public static class RecipeExtensions
{
    public static IRecipeCandidate ConvertFromDtoToModel(this RecipeDto dto)
    {
        if (dto.Ingredients == null || dto.Name == null || dto.Guid == Guid.Empty)
        {
            throw new ArgumentException($"RecipeDto has invalid data: {dto}");
        }

        return new RecipeCandidate(dto.Guid, dto.Name, dto.Ingredients.Select(o => o.ConvertFromDtoToModel()).ToArray());
    }

    public static RecipeDto ConvertFromModelToDto(this IRecipe model)
    {
        return new RecipeDto
        {
            Guid = model.Guid,
            Name = model.RecipeName,
            Ingredients = model.IngredientsForRecipe.Select(o => o.ConvertFromModelToDto()).ToList(),
        };
    }

    
}

[JsonObject("Recipe")]
public sealed class RecipeDto
{
    [JsonProperty("Name")] 
    public string? Name { get; set; }
    
    [JsonProperty("Recipe_Id")] 
    public Guid Guid { get; set; }

    [JsonProperty("IngredientForRecipe")] 
    public List<IngredientForRecipeDto>? Ingredients { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"RecipeDto: [Guid={Guid}] [Name={Name ?? "NULL"} {string.Join(Environment.NewLine, Ingredients != null ? Ingredients : new List<IngredientDto>())}]";
    }
}