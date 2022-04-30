using Newtonsoft.Json;
using PRF.Utils.CoreComponents.Diagnostic;
using VetSolutionRation.DataProvider.Models;

namespace VetSolutionRation.DataProvider.Dto;

public static class DtoExporter
{
    /// <summary>
    /// Export a model into a dto
    /// </summary>
    public static ReferenceDataFileDto ExportToDto(this IInraRationTableImportModel model)
    {
        return new ReferenceDataFileDto
        {
            Lines = CreateLinesDto(model.GetAllLines())
        };
    }
    
    public static string SerializeReferenceToJson(this ReferenceDataFileDto model)
    {
        return JsonConvert.SerializeObject(model);
    }
    
    public static ReferenceDataFileDto DeserializeFromJson(string json)
    {
        return JsonConvert.DeserializeObject<ReferenceDataFileDto>(json) ?? throw new ArgumentException("DeSerializeFromJson: null result");
    }

    private static List<ReferenceLineDto> CreateLinesDto(IReadOnlyCollection<IInraRationLineImportModel> lineModels)
    {
        return lineModels.Select(o => new ReferenceLineDto
        {
            Labels = o.GetLabels().ToList(),
            Cells = ConvertToDto(o.GetAllCells()),
        }).ToList();
    }

    private static List<CellDto> ConvertToDto(IReadOnlyDictionary<InraHeader,FeedCellModel> cells)
    {
        return cells.Select(o => new CellDto
        {
            CellContent = o.Value.Content,
            HeaderKind = o.Key.ToDtoKey()
        }).ToList();
    }

    /// <summary>
    /// Import a dto into a model
    /// </summary>
    public static IInraRationTableImportModel ImportFromDto(this ReferenceDataFileDto dto)
    {
        if (dto.Lines == null)
        {
            return new InraRationTableImportModel(Array.Empty<IInraRationLineImportModel>());
        }

        List<IInraRationLineImportModel> lines = new List<IInraRationLineImportModel>();
        foreach (var line in dto.Lines)
        {
            var labels = line.Labels ?? throw new InvalidOperationException("no label for the line");
            lines.Add(new InraRationLineImportModel(ConvertCells(line), labels));
        }

        return new InraRationTableImportModel(lines);
    }

    private static IReadOnlyCollection<FeedCellModel> ConvertCells(ReferenceLineDto line)
    {
        if (line.Cells == null)
        {
            DebugCore.Fail($"no cell retrieve for line {line}");
            return Array.Empty<FeedCellModel>();
        }

        var lines = new List<FeedCellModel>();
        foreach (var cell in line.Cells)
        {
            if (cell.HeaderKind != null && 
                cell.CellContent != null && 
                InraHeaderExtensions.TryParseDtoInraHeader(cell.HeaderKind, out var inraHeader))
            {
                lines.Add(new FeedCellModel(inraHeader, cell.CellContent));
            }
        }
        return lines;
    }
}