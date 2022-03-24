using System.Diagnostics;
using VetSolutionRation.DataProvider.Models.SubParts;
using VetSolutionRation.DataProvider.Services.Excel.ExcelDto;
using VetSolutionRation.DataProvider.Utils;

namespace VetSolutionRation.DataProvider.Models;

public interface IInraRationTableImportModel
{
}

public sealed class InraRationTableImportModel : IInraRationTableImportModel
{
    private readonly InraHeaderModel _inraHeader;

    public InraRationTableImportModel(ExcelDto excelDto)
    {
        // inra reader has a first row that act as a grouping category:
        var groupingCategory = new InraGroupCategories(excelDto.IgnoredRows.Single());
        _inraHeader = MapInraHeaders(groupingCategory, excelDto.HeaderRowDto);
    }

    private static InraHeaderModel MapInraHeaders(InraGroupCategories groupingCategory, IExcelRowDto getHeaderRow)
    {
        var dto = new InraHeaderModel(groupingCategory);
        foreach (var cellWithPosition in getHeaderRow.Cells)
        {
            if (InraHeaderExtensions.TryParseInraHeader(cellWithPosition.Value, groupingCategory.GuessedCulture, out var inraHeader))
            {
                dto.AddHeader(cellWithPosition.Key, inraHeader);
            }
            else if (LabelRegexHolder.Match(cellWithPosition.Value, out var labelIndex))
            {
                dto.AddLabelPart(labelIndex, cellWithPosition.Value);
            }
            else
            {
                Debug.Fail($"unknown inra header : [{cellWithPosition.Value}] at position {cellWithPosition.Key}");
            }
        }

        return dto;
    }

    private sealed class InraHeaderModel
    {
        private readonly InraGroupCategories _inraGroupCategories;
        private readonly Dictionary<HeaderGroup, Dictionary<InraHeader, int>> _columnIndexByGroupAndHeader = new Dictionary<HeaderGroup,Dictionary<InraHeader, int>>();
        private readonly Dictionary<int, string> _labelParts = new Dictionary<int, string>();

        public InraHeaderModel(InraGroupCategories inraGroupCategories)
        {
            _inraGroupCategories = inraGroupCategories;
            foreach (var groups in inraGroupCategories.OrderedGroups)
            {
                _columnIndexByGroupAndHeader.Add(groups, new Dictionary<InraHeader, int>());
            }
        }

        public void AddHeader(int columnIndex, InraHeader inraHeader)
        {
            try
            {
                _columnIndexByGroupAndHeader[_inraGroupCategories.GetGroupByIndex(columnIndex)].Add(inraHeader, columnIndex);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void AddLabelPart(int labelPosition, string value)
        {
            if (!_labelParts.ContainsKey(labelPosition))
            {
                _labelParts.Add(labelPosition, value);
            }
            else
            {
                Debug.Fail($"duplicate label position for {labelPosition} [{value}]");
            }
        }
    }
}