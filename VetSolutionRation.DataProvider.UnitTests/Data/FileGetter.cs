using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace VetSolutionRation.DataProvider.UnitTests.Data;

// ReSharper disable InconsistentNaming
internal enum AvailableFile
{
    InraRatioConcentrateTable_FR,
    InraRatioForageTable_FR,
    InraRatioConcentrateTable_EN,
    InraRatioForageTable_EN,
}
// ReSharper restore InconsistentNaming

internal static class FileGetter
{
    private static readonly Dictionary<string,FileInfo> _files;

    static FileGetter()
    {
        var dataDirectory = new DirectoryInfo(Path.Combine(TestContext.CurrentContext.TestDirectory ?? throw new InvalidOperationException(), "Data"));
        _files = dataDirectory.GetFiles().ToDictionary(o => o.Name);
    }
    
    public static FileInfo GetFile(AvailableFile availableFile)
    {
        if (_files.TryGetValue(availableFile.GetMatchingFileName(), out var file))
        {
            return file;
        }
        throw new ArgumentException("the file {availableFile.GetMatchingFileName()} has not been found in Data directory");
    }

    private static string GetMatchingFileName(this AvailableFile availableFile)
    {
        switch (availableFile)
        {
            case AvailableFile.InraRatioConcentrateTable_FR:
                return @"INRA2018_TablesConcentres_20122018.xlsx";
            case AvailableFile.InraRatioForageTable_FR:
                return @"INRA2018_TablesFourrages_17042019.xlsx";
            case AvailableFile.InraRatioConcentrateTable_EN:
                return @"INRA2018_ConcentrateFeedTables_08022018.xlsx";
            case AvailableFile.InraRatioForageTable_EN:
                return @"INRA2018_ForageFeedTables_17042019.xlsx";
            default:
                throw new ArgumentOutOfRangeException(nameof(availableFile), availableFile, null);
        }
    }
}