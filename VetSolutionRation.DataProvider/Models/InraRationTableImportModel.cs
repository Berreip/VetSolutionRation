using VetSolutionRation.DataProvider.Models.Helpers;
using VetSolutionRation.DataProvider.Models.SubParts;
using VetSolutionRation.DataProvider.Services.Excel.ExcelDto;

namespace VetSolutionRation.DataProvider.Models;

public interface IInraRationTableImportModel
{
    /// <summary>
    /// Returns all available label in the file
    /// </summary>
    IReadOnlyList<string> GetLabels();
}

public sealed class InraRationTableImportModel : IInraRationTableImportModel
{
    private readonly ExcelDto _excelDto;
    private readonly InraHeaderModel _inraHeader;
    private readonly HashSet<string> _labels = new HashSet<string>();

    public InraRationTableImportModel(ExcelDto excelDto)
    {
        _excelDto = excelDto;
        // inra reader has a first row that act as a grouping category:
        var groupingCategory = new InraGroupCategories(excelDto.IgnoredRows.Single());
        _inraHeader = InraRationTableImportHelper.MapInraHeaders(groupingCategory, excelDto.HeaderRowDto);

        //build access dictionary
        foreach (var row in excelDto.Rows)
        {
            var labels = _inraHeader
                .GetLabelPositions()
                .Select(i => row.GetContent(i))
                .Where(o => !string.IsNullOrWhiteSpace(o));
            var labelJoined = string.Join(" | ", labels);
            _labels.Add(labelJoined);
        }
    }


    /// <inheritdoc />
    public IReadOnlyList<string> GetLabels()
    {
        return _labels.ToArray();
    }
}