using Newtonsoft.Json;

namespace VetSolutionRation.DataProvider.Dto;

/// <summary>
/// Represent a dto that could be saved or exported and that contains all available information about all feeds
/// </summary>
[JsonObject("ReferenceDataFileDto")]
public sealed class ReferenceDataFileDto
{
    [JsonProperty("Lines")]
    public List<ReferenceLineDto>? Lines { get; set; }
}

[JsonObject("ReferenceLineDto")]
public sealed class ReferenceLineDto
{
    [JsonProperty("Labels")]
    public List<string>? Labels { get; set; }
    
    [JsonProperty("Cells")]
    public List<CellDto>? Cells { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        if (Labels == null)
        {
            return "NULL LABELS LINE";
        }
        return $"LINE: {string.Join(" | ", Labels)}";
    }
}

[JsonObject("CellDto")]
public class CellDto
{
    [JsonProperty("CellContent")]
    public string? CellContent { get; set;}
    
    [JsonProperty("HeaderKind")]
    public string? HeaderKind { get;set;}
}

