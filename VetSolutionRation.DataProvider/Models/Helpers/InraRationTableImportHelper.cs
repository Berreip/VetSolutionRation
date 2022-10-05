using System.Diagnostics;
using VetSolutionRation.DataProvider.Models.SubParts;
using VetSolutionRation.DataProvider.Services.Excel.ExcelDto;
using VetSolutionRation.DataProvider.Utils;
using VSR.Core.Extensions;

namespace VetSolutionRation.DataProvider.Models.Helpers;

internal static class InraRationTableImportHelper
{
    public static InraHeaderModel MapInraHeaders(InraGroupCategories groupingCategory, IExcelRowDto getHeaderRow)
    {
        var dto = new InraHeaderModel(groupingCategory);
        foreach (var cellWithPosition in getHeaderRow.Cells)
        {
            if (InraHeaderExtensions.TryParseInraHeader(cellWithPosition.Value, groupingCategory.GuessedCulture, out var inraHeader))
            {
                dto.AddHeader(cellWithPosition.Key, inraHeader);
            }
            else if (LabelRegexHolder.Match(cellWithPosition.Value, out _))
            {
                dto.AddLabelPart(cellWithPosition.Key, cellWithPosition.Value);
            }
            else
            {
                Debug.Fail($"unknown inra header : [{cellWithPosition.Value}] at position {cellWithPosition.Key}");
            }
        }

        return dto;
    }
}