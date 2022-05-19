using Newtonsoft.Json;
using VetSolutionRationLib.Enums;
using VetSolutionRationLib.Models.Feed;

namespace VetSolutionRation.DataProvider.Dto;

public static class DtoExporter
{
    /// <summary>
    /// Export a model into a dto
    /// </summary>
    public static ReferenceDataDto ConvertToDto(this IEnumerable<IFeed> feeds)
    {
        return new ReferenceDataDto
        {
            Feeds = CreateLinesDto(feeds)
        };
    }

    public static string SerializeReferenceToJson(this ReferenceDataDto model)
    {
        return JsonConvert.SerializeObject(model);
    }

    public static ReferenceDataDto DeserializeFromJson(string json)
    {
        return JsonConvert.DeserializeObject<ReferenceDataDto>(json) ?? throw new ArgumentException("DeSerializeFromJson: null result");
    }

    private static List<FeedDto> CreateLinesDto(IEnumerable<IFeed> feeds)
    {
        return feeds.Select(o => new FeedDto
        {
            IsReferenceFeed = o is IReferenceFeed,
            Labels = o.GetLabels(),
            NutritionDetails = ConvertToDto(o.NutritionalDetails),
            StringDetails = ConvertToDto(o.StringDetailsContent),
        }).ToList();
    }

    private static List<StringDetailDto> ConvertToDto(IReadOnlyList<IStringDetailsContent> stringDetailsContent)
    {
        return stringDetailsContent
            .Select(o => new StringDetailDto { CellContent = o.Details, HeaderKind = o.Header.ToDtoKey() })
            .ToList();
    }

    private static List<NutritionDetailDto> ConvertToDto(IReadOnlyList<INutritionalFeedDetails> nutritionalDetails)
    {
        return nutritionalDetails
            .Select(o => new NutritionDetailDto { CellContent = o.Value, HeaderKind = o.Header.ToDtoKey() })
            .ToList();
    }

    /// <summary>
    /// Import a dto into a model
    /// </summary>
    public static IFeed ConvertFromDto(this FeedDto dto)
    {
        if (dto.IsReferenceFeed)
        {
            return new ReferenceFeed(
                dto.Labels ?? throw new InvalidOperationException(),
                ConvertFromDto(dto.NutritionDetails),
                ConvertFromDto(dto.StringDetails));
        }

        return new CustomFeed(
            dto.Labels ?? throw new InvalidOperationException(),
            ConvertFromDto(dto.NutritionDetails));
    }

    private static IEnumerable<IStringDetailsContent> ConvertFromDto(IEnumerable<StringDetailDto>? dtoStringDetails)
    {
        var converted = new List<IStringDetailsContent>();
        if (dtoStringDetails == null)
        {
            return converted;
        }

        foreach (var detail in dtoStringDetails)
        {
            if (detail.HeaderKind != null &&
                detail.CellContent != null &&
                InraHeaderExtensions.TryParseDtoInraHeader(detail.HeaderKind, out var inraHeader))
            {
                converted.Add(new StringDetailsContent(inraHeader, detail.CellContent));
            }
        }
        return converted;
    }

    private static IEnumerable<INutritionalFeedDetails> ConvertFromDto(IEnumerable<NutritionDetailDto>? nutritionDetail)
    {
        var converted = new List<INutritionalFeedDetails>();
        if (nutritionDetail == null)
        {
            return converted;
        }

        foreach (var detail in nutritionDetail)
        {
            if (detail.HeaderKind != null &&
                detail.CellContent != null &&
                InraHeaderExtensions.TryParseDtoInraHeader(detail.HeaderKind, out var inraHeader))
            {
                converted.Add(new NutritionalFeedDetails(inraHeader, detail.CellContent.Value));
            }
        }

        return converted;
    }
    //
    // /// <summary>
    // /// Import a dto into a model
    // /// </summary>
    // public static IInraRationTableImportModel ImportFromDto(this ReferenceDataDto dto)
    // {
    //     // TODO PBO delete
    //     if (dto.Feeds == null)
    //     {
    //         return new InraRationTableImportModel(Array.Empty<IInraRationLineImportModel>());
    //     }
    //
    //     List<IInraRationLineImportModel> lines = new List<IInraRationLineImportModel>();
    //     foreach (var line in dto.Feeds)
    //     {
    //         var labels = line.Labels ?? throw new InvalidOperationException("no label for the line");
    //         lines.Add(new InraRationLineImportModel(ConvertCells(line), labels));
    //     }
    //
    //     return new InraRationTableImportModel(lines);
    // }
    //
    // private static IReadOnlyCollection<FeedCellModel> ConvertCells(FeedDto line)
    // {
    //     // TODO PBO delete
    //     if (line.NutritionDetails == null)
    //     {
    //         DebugCore.Fail($"no cell retrieve for line {line}");
    //         return Array.Empty<FeedCellModel>();
    //     }
    //
    //     var lines = new List<FeedCellModel>();
    //     foreach (var cell in line.NutritionDetails)
    //     {
    //         if (cell.HeaderKind != null && 
    //             cell.CellContent != null && 
    //             InraHeaderExtensions.TryParseDtoInraHeader(cell.HeaderKind, out var inraHeader))
    //         {
    //             lines.Add(new FeedCellModel(inraHeader, cell.CellContent));
    //         }
    //     }
    //     return lines;
    // }
}