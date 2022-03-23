namespace VetSolutionRatio.DataProvider.Services.Excel.ExcelDto;

public sealed class ExcelDto
{
    private readonly ExcelRowDto _headerRowDto;
    private readonly List<ExcelRowDto> _rows;

    public ExcelDto(ExcelRowDto headerRowDto, List<ExcelRowDto> rows)
    {
        _headerRowDto = headerRowDto;
        _rows = rows;
    }

    public IExcelRowDto GetHeaderRow()
    {
        return _headerRowDto;
    }
}