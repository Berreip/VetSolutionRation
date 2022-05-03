using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PRF.WPFCore;
using PRF.WPFCore.Browsers;
using PRF.WPFCore.Commands;
using PRF.WPFCore.CustomCollections;
using VetSolutionRation.Common.Async;
using VetSolutionRation.DataProvider;
using VetSolutionRation.DataProvider.Models.Helpers;
using VetSolutionRation.wpf.Helpers;
using VetSolutionRation.wpf.Services.Configuration;
using VetSolutionRation.wpf.Services.Feed;
using VetSolutionRation.wpf.Views.Import.Adapters;
using VetSolutionRationLib.Models.Feed;

namespace VetSolutionRation.wpf.Views.Import;

internal interface IImportViewModel
{
    /// <summary>
    /// load drag and drop files
    /// </summary>
    void LoadDroppedFiles(FileInfo[] matchingFiles);
}

internal sealed class ImportViewModel : ViewModelBase, IImportViewModel
{
    private readonly IFeedProvider _feedProvider;
    private readonly IConfigurationManager _configurationManager;
    public IDelegateCommandLight ImportCommand { get; }
    public IDelegateCommandLight OpenCacheFolderCommand { get; }
    private RegisteredDataAdapter[] _allLoadedData = Array.Empty<RegisteredDataAdapter>();

    private readonly object _key = new object();
    private bool _isIdle = true;
    private RegisteredDataAdapter? _selectedData;
    private RegisteredNutrionalDetailsAdapter[]? _selectedDataDetails;

    public ImportViewModel(IFeedProvider feedProvider, IConfigurationManager configurationManager)
    {
        _feedProvider = feedProvider;
        _configurationManager = configurationManager;
        ImportCommand = new DelegateCommandLight(ExecuteImportCommand);
        OpenCacheFolderCommand = new DelegateCommandLight(ExecuteOpenCacheFolderCommand);
        RefreshAllLoadedData();
        feedProvider.OnNewDataProvided += OnNewDataProvided;
    }

    public RegisteredDataAdapter[] AllLoadedData
    {
        get => _allLoadedData;
        private set => SetProperty(ref _allLoadedData, value);
    }

    private async void ExecuteImportCommand()
    {
        IsIdle = false;
        await AsyncWrapper.DispatchAndWrapAsync(() =>
        {
            var file = BrowserDialogManager.OpenFileBrowser("Fichiers (*.*)|*.*", null);
            if (file != null)
            {
                LoadInraeFile(file);
            }
        }, () => IsIdle = true).ConfigureAwait(false);
    }

    private void ExecuteOpenCacheFolderCommand()
    {
        AsyncWrapper.Wrap(() => { _configurationManager.GetCacheDataFolder().OpenFolderInExplorer(); });
    }
    

    private async void OnNewDataProvided()
    {
        await AsyncWrapper.DispatchAndWrapAsync(RefreshAllLoadedData).ConfigureAwait(false);
    }

    private void RefreshAllLoadedData()
    {
        lock (_key)
        {
            SelectedData = null;
            AllLoadedData = _feedProvider.GetFeeds()
                .Select(o => new RegisteredDataAdapter(o))
                .OrderBy(o => o.LoadedDataLabel)
                .ToArray();
        }
    }

    private void LoadInraeFile(FileInfo file)
    {
        lock (_key)
        {
            if (!FileFeedSourceExtensions.TryParseFromFileName(file.Name, out FileFeedSource feedSource))
            {
                MessageBox.Show($@"Le nom de fichier {file.Name} ne permet pas de lui attribuer une catégorie. il sera considéré comme un fichier de type 'Custom' par défaut");
            }

            var inraFile = InraRatioTableImporter.ImportInraTable(file);

            // for reference feeds:
            if (feedSource == FileFeedSource.Reference)
            {
                _feedProvider.AddFeeds(inraFile.GetAllLines().Select(o => o.ToReferenceFeed()));
            }
            else
            {
                _feedProvider.AddFeeds(inraFile.GetAllLines().Select(o => o.ToCustomFeed()));
            }
        }
    }

    public bool IsIdle
    {
        get => _isIdle;
        set
        {
            if (SetProperty(ref _isIdle, value))
            {
                ImportCommand.RaiseCanExecuteChanged();
                OpenCacheFolderCommand.RaiseCanExecuteChanged();
            }
        }
    }

    public RegisteredDataAdapter? SelectedData
    {
        get => _selectedData;
        set
        {
            if (SetProperty(ref _selectedData, value))
            {
                if (_selectedData == null)
                {
                    SelectedDataDetails = null;
                }
                else
                {
                    SelectedDataDetails = _selectedData.GetDetails();
                }
            }
        }
    }

    public RegisteredNutrionalDetailsAdapter[]? SelectedDataDetails
    {
        get => _selectedDataDetails;
        private set => SetProperty(ref _selectedDataDetails, value);
    }

    /// <inheritdoc />
    public async void LoadDroppedFiles(FileInfo[] matchingFiles)
    {
        IsIdle = false;
        await AsyncWrapper.DispatchAndWrapAsync(() =>
        {
            foreach (var file in matchingFiles)
            {
                LoadInraeFile(file);
            }
        }, () => IsIdle = true).ConfigureAwait(false);
    }
}