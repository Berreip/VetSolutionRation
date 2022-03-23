using VetSolutionRatio.DataProvider.Dto;
using VetSolutionRatio.DataProvider.Services.Excel;

namespace VetSolutionRatio.DataProvider;

/// <summary>
/// Class that import a ratio table from Inra
/// </summary>
public static class InraRatioTableImporter
{
    /// <summary>
    /// Import the given file
    /// </summary>
    public static IInraRationTableImportDto ImportInraTable(FileInfo inraFile)
    {
        if (!inraFile.Exists)
        {
            throw new ArgumentException($"the provided file {inraFile.FullName} does not exists");
        }

        var excelDto = ExcelProvider.ImportExcel(inraFile, 1);
       
        return new InraRationTableImportDto(excelDto);

    }
}
