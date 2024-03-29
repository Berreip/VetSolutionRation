﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PRF.WPFCore;
using PRF.WPFCore.Browsers;
using PRF.WPFCore.Commands;
using VetSolutionRation.DataProvider;
using VetSolutionRation.DataProvider.Models.Helpers;
using VSR.Core.Helpers.Async;
using VSR.Core.Services;
using VSR.WPF.Utils.Adapters.ImportPanel;
using VSR.WPF.Utils.Helpers;
using VSR.WPF.Utils.Services.Configuration;

namespace VetSolutionRation.wpf.ImportView;

internal interface IImportViewModel
{
    /// <summary>
    /// load drag and drop files
    /// </summary>
    void LoadDroppedFiles(FileInfo[] matchingFiles);
}

internal sealed class ImportViewModel : ViewModelBase, IImportViewModel
{
    private readonly IIngredientsManager _ingredientsManager;
    private readonly IConfigurationManager _configurationManager;
    public IDelegateCommandLight ImportCommand { get; }
    public IDelegateCommandLight OpenCacheFolderCommand { get; }
    private RegisteredDataAdapter[] _allLoadedData = Array.Empty<RegisteredDataAdapter>();

    private readonly object _key = new object();
    private bool _isIdle = true;
    private RegisteredDataAdapter? _selectedData;
    private IReadOnlyCollection<RegisteredNutrionalDetailsAdapter>? _selectedDataDetails;

    public ImportViewModel(IIngredientsManager ingredientsManager, IConfigurationManager configurationManager)
    {
        _ingredientsManager = ingredientsManager;
        _configurationManager = configurationManager;
        ImportCommand = new DelegateCommandLight(ExecuteImportCommand);
        OpenCacheFolderCommand = new DelegateCommandLight(ExecuteOpenCacheFolderCommand);
        RefreshAllLoadedData();
        ingredientsManager.OnIngredientsChanged += OnNewDataProvided;
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
    

    private async void OnNewDataProvided(IIngredientsChangeMonitor monitor)
    {
        await AsyncWrapper.DispatchAndWrapAsync(RefreshAllLoadedData).ConfigureAwait(false);
    }

    private void RefreshAllLoadedData()
    {
        lock (_key)
        {
            SelectedData = null;
            AllLoadedData = _ingredientsManager.GetAllIngredients()
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
                _ingredientsManager.AddIngredients(inraFile.GetAllLines().Select(o => o.ToReferenceIngredient()).ToArray());
            }
            else
            {
                _ingredientsManager.AddIngredients(inraFile.GetAllLines().Select(o => o.ToUserDefinedIngredient()).ToArray());
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

    public IReadOnlyCollection<RegisteredNutrionalDetailsAdapter>? SelectedDataDetails
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