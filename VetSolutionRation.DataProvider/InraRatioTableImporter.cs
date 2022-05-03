using DocumentFormat.OpenXml.Spreadsheet;
using VetSolutionRation.DataProvider.Models;
using VetSolutionRation.DataProvider.Models.Helpers;
using VetSolutionRation.DataProvider.Models.SubParts;
using VetSolutionRation.DataProvider.Services.Excel;
using VetSolutionRation.DataProvider.Services.Excel.ExcelDto;
using VetSolutionRationLib.Enums;

namespace VetSolutionRation.DataProvider;

/// <summary>
/// Class that import a ratio table from Inra
/// </summary>
public static class InraRatioTableImporter
{
    /// <summary>
    /// Import the given file
    /// </summary>
    public static IInraRationTableImportModel ImportInraTable(FileInfo inraFile)
    {
        if (!inraFile.Exists)
        {
            throw new ArgumentException($"the provided file {inraFile.FullName} does not exists");
        }

        var excelDto = ExcelProvider.ImportExcel(inraFile, 1);

        return excelDto.BuildFromExcelDto();
    }

    public static IInraRationTableImportModel BuildFromExcelDto(this ExcelDto excelDto)
    {
        // inra reader has a first row that act as a grouping category:
        var groupingCategory = new InraGroupCategories(excelDto.IgnoredRows.Single());
        var inraHeader = InraRationTableImportHelper.MapInraHeaders(groupingCategory, excelDto.HeaderRowDto);

        var linesModel = new List<InraRationLineImportModel>();
        //build access dictionary
        for (var index = 0; index < excelDto.Rows.Count; index++)
        {
            var row = excelDto.Rows[index];
            var labels = GetHeaders(inraHeader, row);

            if (labels.Count != 0)
            {
                linesModel.Add(new InraRationLineImportModel(GetCellsValues(inraHeader, row), labels));
            }
        }

        return new InraRationTableImportModel(linesModel);
    }

    private static IReadOnlyCollection<string> GetHeaders(InraHeaderModel inraHeader, IExcelRowDto row)
    {
        var labels = new List<string>();
        foreach (var labelPosition in inraHeader.GetLabelPositions())
        {
            if (row.TryGetContent(labelPosition, out var content))
            {
                labels.Add(content);
            }
        }

        return labels;
    }

    private static IReadOnlyCollection<FeedCellModel> GetCellsValues(InraHeaderModel inraHeader, IExcelRowDto row)
    {
        var cells = new List<FeedCellModel>();
        foreach (var definedHeader in inraHeader.GetDefinedHeaders())
        {
            if (row.TryGetContent(definedHeader.HeaderPosition, out var content))
            {
                cells.Add(new FeedCellModel(definedHeader.HeaderKind, content));
            }
        }
        return cells;
    }
}

public sealed class FeedCellModel
{
    public InraHeader HeaderKind { get; }
    public string Content { get; }

    public FeedCellModel(InraHeader headerKind, string content)
    {
        HeaderKind = headerKind;
        Content = content;
    }

    /// <summary>
    /// Returns true if content are the same
    /// </summary>
    public bool Match(FeedCellModel cell)
    {
        return cell.Content == Content;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"[{HeaderKind}]={Content}";
    }

    /// <summary>
    /// Some values should be ignore because there is a lots of wrong input in the inrae tables
    /// </summary>
    public bool IsContentIgnorableWhenDuplicates()
    {
        return Content == "0";
    }
}