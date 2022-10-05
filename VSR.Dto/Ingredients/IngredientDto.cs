using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using VSR.Models.Ingredients;

namespace VSR.Dto.Ingredients;

public static class IngredientExtensions
{
    public static IIngredient ConvertFromDtoToModel(this IngredientDto dto)
    {
        return new Ingredient(
            dto.Guid,
            dto.Label ?? "N/A",
            dto.IsUserAdded,
            dto.NutritionDetails?.Select(o => o.ConvertFromDtoToModel()).ToArray() ?? Array.Empty<INutritionalDetails>());
    }

    public static IngredientDto ConvertFromModelToDto(this IIngredient model)
    {
        return new IngredientDto
        {
            Label = model.Label,
            Guid = model.Guid,
            NutritionDetails = model.GetNutritionDetails().Select(o => o.ConvertFromModelToDto()).ToList(),
            IsUserAdded = model.IsUserAdded,
        };
    }
}

[JsonObject("IngredientDto")]
public sealed class IngredientDto
{
    /// <summary>
    /// Label
    /// </summary>
    [JsonProperty("Label")]
    public string? Label { get; set; }

    /// <summary>
    /// the unique identifier for this Feed
    /// </summary>
    [JsonProperty("Guid")]
    public Guid Guid { get; set; }

    /// <summary>
    /// Feed details characteristics
    /// </summary>
    [JsonProperty("Nutrition")]
    public List<NutritionalDetailDto>? NutritionDetails { get; set; }

    /// <summary>
    /// whether this feed is from a reference file or from a user
    /// </summary>
    [JsonProperty("IsReferenceFeed")]
    public bool IsUserAdded { get; set; }
}