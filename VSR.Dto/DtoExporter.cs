using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using VSR.Dto.Ingredients;
using VSR.Dto.Recipe;
using VSR.Models.Ingredients;
using VSR.Models.Recipe;

namespace VSR.Dto;

/// <summary>
/// Represent a dto that could be saved or exported and that contains all available information about all feeds
/// </summary>
[JsonObject("ReferenceDataDto")]
public sealed class ReferenceDataDto
{
    [JsonProperty("Ingredients")]
    public List<IngredientDto>? Ingredients { get; set; }

    [JsonProperty("Recipes")]
    public List<RecipeDto>? Recipes { get; set; }
}

public static class DtoExporter
{
    /// <summary>
    /// Export a model into a dto
    /// </summary>
    public static ReferenceDataDto ConvertFromModelsToDto(IEnumerable<IIngredient> ingredients, IEnumerable<IRecipe> recipes)
    {
        return new ReferenceDataDto
        {
            Ingredients = ingredients.Select(o => o.ConvertFromModelToDto()).ToList(),
            Recipes = recipes.Select(o => o.ConvertFromModelToDto()).ToList(),
        };
    }
    
    public static IReadOnlyCollection<IIngredient> ConvertFromDtoToIngredients(this ReferenceDataDto dto)
    {
        return dto.Ingredients?.Select(o => o.ConvertFromDtoToModel()).ToArray() ?? Array.Empty<IIngredient>();
    }

    public static IReadOnlyCollection<IRecipeCandidate> ConvertFromDtoToRecipeCandidates(this ReferenceDataDto dto)
    {
        return dto.Recipes?.Select(o => o.ConvertFromDtoToModel()).ToArray() ?? Array.Empty<IRecipeCandidate>();
    }

    public static string SerializeReferenceToJson(this ReferenceDataDto model)
    {
        return JsonConvert.SerializeObject(model);
    }

    public static ReferenceDataDto DeserializeFromJson(string json)
    {
        return JsonConvert.DeserializeObject<ReferenceDataDto>(json) ?? throw new ArgumentException("DeSerializeFromJson: null result");
    }
}