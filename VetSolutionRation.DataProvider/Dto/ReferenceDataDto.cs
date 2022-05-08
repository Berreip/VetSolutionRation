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

[JsonObject("StringDetail")]
public sealed class StringDetailDto
{
    [JsonProperty("v")]
    public string? CellContent { get; set;}
    
    [JsonProperty("k")]
    public string? HeaderKind { get;set;}
}

[JsonObject("NutritionDetails")]
public sealed class NutritionDetailDto
{
    [JsonProperty("v")]
    public double? CellContent { get; set;}
    
    [JsonProperty("k")]
    public string? HeaderKind { get;set;}
}

