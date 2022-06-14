using Newtonsoft.Json;
using VetSolutionRationLib.Helpers;

namespace VetSolutionRation.DataProvider.Dto;

/// <summary>
/// Represent a dto that could be saved or exported and that contains all available information about all feeds
/// </summary>
[JsonObject("ReferenceDataDto")]
public sealed class ReferenceDataDto
{
    [JsonProperty("Feeds")]
    public List<FeedDto>? Feeds { get; set; }

    [JsonProperty("Recipes")]
    public List<RecipeDto>? Recipes { get; set; }
}

[JsonObject("ReferenceLineDto")]
public sealed class FeedDto
{
    /// <summary>
    /// All separated label (should at least have one).
    /// Labels are separated by | when displayed
    /// </summary>
    [JsonProperty("Labels")]
    public List<string>? Labels { get; set; }
    
    /// <summary>
    /// Feed details characteristics
    /// </summary>
    [JsonProperty("Nutrition")]
    public List<NutritionDetailDto>? NutritionDetails { get; set; }
    
    /// <summary>
    /// Feed string details characteristics
    /// </summary>
    [JsonProperty("Details")]
    public List<StringDetailDto>? StringDetails { get; set; }

    /// <summary>
    /// whether this feed is from a reference file or from a user
    /// </summary>
    [JsonProperty("IsReferenceFeed")]
    public bool IsReferenceFeed { get; set;}

    /// <inheritdoc />
    public override string ToString() => Labels.JoinAsLabel();
}

[JsonObject("RecipeDto")]
public sealed class RecipeDto
{
    [JsonProperty("Name")]
    public string? Name { get; set;}
    
    [JsonProperty("UnitLabel")]
    public string? UnitLabel{ get; set;}
    
    [JsonProperty("Ingredients")]
    public List<IngredientDto>? Ingredients { get; set;}
    
    /// <inheritdoc />
    public override string ToString()
    {
        return $"RecipeDto: [Name={Name ?? "NULL"} ({UnitLabel ?? "NULL"}) = {string.Join(Environment.NewLine, Ingredients ?? new List<IngredientDto>())}]";
    }
}

[JsonObject("IngredientDto")]
public sealed class IngredientDto
{
    [JsonProperty("Percentage")]
    public double? Percentage { get; set; }
    
    [JsonProperty("FeedsInRecipe")]
    public FeedDto? FeedsInRecipe { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"IngredientDto: [{Percentage}% - {FeedsInRecipe}]";
    }
}

[JsonObject("StringDetail")]
public sealed class StringDetailDto
{
    [JsonProperty("v")]
    public string? CellContent { get; set;}
    
    [JsonProperty("k")]
    public string? HeaderKind { get;set;}
    
    /// <inheritdoc />
    public override string ToString()
    {
        return $"StringDetail: [{HeaderKind} = {CellContent}]";
    }
}

[JsonObject("NutritionDetails")]
public sealed class NutritionDetailDto
{
    [JsonProperty("v")]
    public double? CellContent { get; set;}
    
    [JsonProperty("k")]
    public string? HeaderKind { get;set;}
    
    /// <inheritdoc />
    public override string ToString()
    {
        return $"StringDetail: [{HeaderKind} = {CellContent}]";
    }
}

