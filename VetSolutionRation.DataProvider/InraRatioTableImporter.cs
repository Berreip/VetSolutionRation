﻿using VetSolutionRation.DataProvider.Models;
using VetSolutionRation.DataProvider.Services.Excel;

namespace VetSolutionRation.DataProvider;

/// <summary>
/// Class that import a ratio table from Inra
/// </summary>
public static class InraRatioTableImporter
{
    /// <summary>
    /// Import the given file
    /// </summary>
    public static IInraRationTableImportModel ImportInraTable(FileInfo inraFile)
    {
        if (!inraFile.Exists)
        {
            throw new ArgumentException($"the provided file {inraFile.FullName} does not exists");
        }

        var excelDto = ExcelProvider.ImportExcel(inraFile, 1);
       
        return new InraRationTableImportModel(excelDto);

    }
}
