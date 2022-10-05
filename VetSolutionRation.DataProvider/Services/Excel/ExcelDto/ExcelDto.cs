namespace VetSolutionRation.DataProvider.Services.Excel.ExcelDto;

public sealed class ExcelDto
{
    public IReadOnlyList<IExcelRowDto> IgnoredRows { get; }
    public IExcelRowDto HeaderRowDto { get; }
    public IReadOnlyList<IExcelRowDto> Rows { get; }

    public ExcelDto(IExcelRowDto headerRowDto, IReadOnlyList<ExcelRowDto> rows, IReadOnlyList<ExcelRowDto> ignoredRows)
    {
        IgnoredRows = ignoredRows;
        HeaderRowDto = headerRowDto;
        Rows = rows;
    }
}