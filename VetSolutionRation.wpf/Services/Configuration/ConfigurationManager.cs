using System;
using System.IO;
using PRF.Utils.CoreComponents.Extensions;
using VetSolutionRation.wpf.Helpers;

namespace VetSolutionRation.wpf.Services.Configuration;

public interface IConfigurationManager
{
    DirectoryInfo GetCacheDataFolder();
}

internal sealed class ConfigurationManager : IConfigurationManager
{
    private readonly DirectoryInfo _cacheDataFolder;

    public ConfigurationManager()
    {
        _cacheDataFolder = new DirectoryInfo(
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                VetSolutionRatioConstants.VET_SOLUTION_RATIO_FOLDER,
                VetSolutionRatioConstants.CACHE_FOLDER,
                VetSolutionRatioConstants.LOADED_DATA_CACHE_FOLDER));
        _cacheDataFolder.CreateIfNotExist();
    }
    
    /// <inheritdoc />
    public DirectoryInfo GetCacheDataFolder() => _cacheDataFolder;
}