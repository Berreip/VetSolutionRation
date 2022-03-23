namespace VetSolutionRation.DataProvider.Services.Excel.ExcelDto;

public interface IExcelRowDto
{
    /// <summary>
    /// All cells in the row
    /// </summary>
    IReadOnlyDictionary<int, string> Cells { get; }
}

public sealed class ExcelRowDto : IExcelRowDto
{
    /// <inheritdoc />
    public IReadOnlyDictionary<int, string> Cells => _row;
    
    private readonly Dictionary<int, string> _row = new Dictionary<int, string>();

    public void AddCell(int cellposition, string cellValue)
    {
        _row.Add(cellposition, cellValue);
    }

}