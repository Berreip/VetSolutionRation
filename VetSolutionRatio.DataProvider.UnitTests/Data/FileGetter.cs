using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace VetSolutionRatio.DataProvider.UnitTests.Data;

internal enum AvailableFile
{
    InraRatioConcentrateTable,
    InraRatioForageTable,
}

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
            case AvailableFile.InraRatioConcentrateTable:
                return @"INRA2018_TablesConcentres_20122018.xlsx";
            case AvailableFile.InraRatioForageTable:
                return @"INRA2018_TablesFourrages_17042019.xlsx";
            default:
                throw new ArgumentOutOfRangeException(nameof(availableFile), availableFile, null);
        }
    }
}