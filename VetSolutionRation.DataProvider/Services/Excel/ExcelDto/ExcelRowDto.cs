using System.Diagnostics.CodeAnalysis;

namespace VetSolutionRation.DataProvider.Services.Excel.ExcelDto;

public interface IExcelRowDto
{
    /// <summary>
    /// All cells in the row
    /// </summary>
    IReadOnlyDictionary<int, string> Cells { get; }

    /// <summary>
    /// Returns the value of a cell if it exists
    /// </summary>
    bool TryGetContent(int position, [MaybeNullWhen(false)] out string content);
}

public sealed class ExcelRowDto : IExcelRowDto
{
    /// <inheritdoc />
    public IReadOnlyDictionary<int, string> Cells => _row;

    /// <inheritdoc />
    public bool TryGetContent(int position, [MaybeNullWhen(false)] out string content)
    {
        if(_row.TryGetValue(position, out var str))
        {
            content = str;
            return true;
        }

        content = null;
        return false;

    }

    private readonly Dictionary<int, string> _row = new Dictionary<int, string>();

    public void AddCell(string cellValue, int columnPosition)
    {
        _row.Add(columnPosition, cellValue);
    }

}