using System;
using Newtonsoft.Json;
using VSR.Dto.Utils;
using VSR.Models.Ingredients;

namespace VSR.Dto.Ingredients;

public static class NutritionalDetailExtensions
{
    public static INutritionalDetails ConvertFromDtoToModel(this NutritionalDetailDto dto)
    {
        if (InraHeaderDtoConverter.TryParseDtoInraHeader(dto.HeaderKind, out var inraHeader))
        {
            return new NutritionalDetails(inraHeader, dto.Value);
        }

        throw new ArgumentException($"unable to convert HeaderKind: {dto.HeaderKind}");
    }

    public static NutritionalDetailDto ConvertFromModelToDto(this INutritionalDetails model)
    {
        return new NutritionalDetailDto
        {
            Value = model.Value,
            HeaderKind = model.Header.ToDtoKey(),
        };
    }
}

[JsonObject("NutritionDetails")]
public sealed class NutritionalDetailDto
{
    [JsonProperty("v")] 
    public double Value { get; set; }

    [JsonProperty("k")] 
    public string? HeaderKind { get; set; }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"StringDetail: [{HeaderKind} = {Value}]";
    }
}