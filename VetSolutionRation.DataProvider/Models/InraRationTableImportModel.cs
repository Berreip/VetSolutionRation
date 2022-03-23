using System.Diagnostics;
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
        _inraHeader = MapInraHeaders(excelDto.GetHeaderRow());
    }

    private static InraHeaderModel MapInraHeaders(IExcelRowDto getHeaderRow)
    {
        var dto = new InraHeaderModel();
        foreach (var cellWithPosition in getHeaderRow.Cells)
        {
            if (InraHeaderExtensions.TryParseInraHeader(cellWithPosition.Value, out var inraHeader))
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
        private readonly Dictionary<InraHeader, int> _columnIndexByHeader = new Dictionary<InraHeader, int>();
        private readonly Dictionary<int, string> _labelParts = new Dictionary<int, string>();

        public void AddHeader(int columnIndex, InraHeader inraHeader)
        {
            _columnIndexByHeader.Add(inraHeader, columnIndex);
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