using System.Diagnostics.CodeAnalysis;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using PRF.Utils.CoreComponents.Diagnostic;
using VetSolutionRation.DataProvider.Services.Excel.ExcelDto;
using VetSolutionRation.DataProvider.Services.Excel.ExcelUtils;

namespace VetSolutionRation.DataProvider.Services.Excel;

public static class ExcelProvider
{
    public static ExcelDto.ExcelDto ImportExcel(FileInfo excelFile, int headerRow)
    {
        if (!excelFile.Exists)
        {
            throw new ArgumentException($"the provided file {excelFile.FullName} does not exists");
        }

        using (var fs = new FileStream(excelFile.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        using (var doc = SpreadsheetDocument.Open(fs, false))
        {
            var workbookPart = doc.WorkbookPart ?? throw new InvalidOperationException();
            var sst = workbookPart.GetPartsOfType<SharedStringTablePart>().First().SharedStringTable;
            var sheet = workbookPart.WorksheetParts.First().Worksheet;

            var currentRow = 0;

            ExcelRowDto? headerRowDto = null;
            var rows = new List<ExcelRowDto>();
            var ignoredRows = new List<ExcelRowDto>();
            // iterate over each row:
            foreach (var row in sheet.Descendants<Row>())
            {
                if (headerRow <= currentRow)
                {
                    var rowDto = GetRow(row, sst);
                    if (headerRow == currentRow)
                    {
                        headerRowDto = rowDto;
                    }
                    else
                    {
                        rows.Add(rowDto);
                    }
                }
                else
                {
                    ignoredRows.Add(GetRow(row, sst));
                }

                currentRow++;
            }

            if (headerRowDto != null)
            {
                return new ExcelDto.ExcelDto(headerRowDto, rows, ignoredRows);
            }

            throw new ArgumentException($"Unable too find any row header for the provided file {excelFile.FullName}");
        }

    }

    private static ExcelRowDto GetRow(Row row, SharedStringTable sst)
    {
        var rowDto =  new ExcelRowDto();
        foreach (var cell in row.Elements<Cell>())
        {
            if (cell.CellReference != null && cell.CellReference.Value != null)
            {
                string header = cell.CellReference.Value;
                if (TryGetCellValue(sst, cell, out var cellValue))
                {
                    var columnPosition = ExcelHelpers.GetColumnPositionFromCellReference(header);
                    rowDto.AddCell(cellValue, columnPosition);
                }
            }
            else
            {
                DebugCore.Fail($"celle {cell} has no header");
            }
        }

        return rowDto;
    }

    private static bool TryGetCellValue(SharedStringTable sst, Cell c, [MaybeNullWhen(false)] out string cellValue)
    {
        if (c.CellValue != null)
        {
            if (c.DataType != null && c.DataType == CellValues.SharedString)
            {
                var ssid = int.Parse(c.CellValue.Text);
                cellValue = sst.ChildElements[ssid].InnerText;
                return true;
            }

            // else:
            cellValue = c.CellValue.Text;
            return true;
        }

        cellValue = null;
        return false;
    }
}