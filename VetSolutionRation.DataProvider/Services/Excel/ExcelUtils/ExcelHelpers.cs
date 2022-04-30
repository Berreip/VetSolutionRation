using System.Text.RegularExpressions;

namespace VetSolutionRation.DataProvider.Services.Excel.ExcelUtils;

/// <summary>
/// Stupid class to extract data from this fucking piece of shit of excel openxml model...
/// </summary>
public static class ExcelHelpers
{
    private static readonly Regex _headerRegex = new Regex("[A-Za-z]+", RegexOptions.Compiled);

    private const int ADDITIONAL_CYCLE_HEADER = 26;
    private static readonly Dictionary<char, int> _excelUpperHeaderToPosition;

    static ExcelHelpers()
    {
        _excelUpperHeaderToPosition = new Dictionary<char, int>
        {
            { 'A', 1 },
            { 'B', 2 },
            { 'C', 3 },
            { 'D', 4 },
            { 'E', 5 },
            { 'F', 6 },
            { 'G', 7 },
            { 'H', 8 },
            { 'I', 9 },
            { 'J', 10 },
            { 'K', 11 },
            { 'L', 12 },
            { 'M', 13 },
            { 'N', 14 },
            { 'O', 15 },
            { 'P', 16 },
            { 'Q', 17 },
            { 'R', 18 },
            { 'S', 19 },
            { 'T', 20 },
            { 'U', 21 },
            { 'V', 22 },
            { 'W', 23 },
            { 'X', 24 },
            { 'Y', 25 },
            { 'Z', 26 },
        };
    }

    /// <summary>
    /// Returns an index position starting at zero (like any not too stupid indexing (except excel of course...))
    /// from the complete cell reference using slow, inneficient and error prone way (but the only one)
    /// => From A1 => return 0, from A2, return 0 (same column)
    /// => From B123 => return 1 (first column)
    /// </summary>
    public static int GetColumnPositionFromCellReference(string cellReference)
    {
        Match match = _headerRegex.Match(cellReference);
        if (!match.Success)
        {
            throw new ArgumentException($"the cell reference {cellReference} is not valid. it should be formated like A1, B4, AA2...");
        }

        return ConvertExcelHeaderToIndex(match.Value.ToUpperInvariant());
    }

    private static int ConvertExcelHeaderToIndex(string header)
    {
        var index = 0;
        var cycle = 0;
        for (var i = header.Length - 1; i >= 0; i--)
        {
            var charHeader = header[i];
            var letterPosition = _excelUpperHeaderToPosition[charHeader];
            index += letterPosition * (int)Math.Pow(ADDITIONAL_CYCLE_HEADER, cycle);
            cycle++;
        }

        return index - 1; // zero base the index
    }
}