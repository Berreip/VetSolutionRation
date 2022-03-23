using System.Diagnostics;
using VetSolutionRation.DataProvider.Services.Excel.ExcelDto;

namespace VetSolutionRation.DataProvider.Dto;

public interface IInraRationTableImportDto
{
}

public sealed class InraRationTableImportDto : IInraRationTableImportDto
{
    public InraRationTableImportDto(ExcelDto excelDto)
    {
        var inraMapHeader = MapInraHeaders(excelDto.GetHeaderRow());
    }

    private static Dictionary<InraHeader, int> MapInraHeaders(IExcelRowDto getHeaderRow)
    {
        var inraHeaderRow = new Dictionary<InraHeader, int>();
        foreach (var cellWithPosition in getHeaderRow.Cells)
        {
            if (InraHeaderExtensions.TryParseInraHeader(cellWithPosition.Value, out var inraHeader))
            {
                inraHeaderRow.Add(inraHeader, cellWithPosition.Key);
            }
            else
            {
                Debug.Fail($"unknown inra header : [{cellWithPosition.Value}] at position {cellWithPosition.Key}");
            }
        }

        return inraHeaderRow;
    }
}
