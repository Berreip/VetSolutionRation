using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PRF.Utils.CoreComponents.Diagnostic;
using PRF.WPFCore;
using PRF.WPFCore.Browsers;
using PRF.WPFCore.Commands;
using PRF.WPFCore.CustomCollections;
using VetSolutionRation.Common.Async;
using VetSolutionRation.DataProvider;
using VetSolutionRation.DataProvider.Models;
using VetSolutionRation.wpf.Helpers;
using VetSolutionRation.wpf.Services.Configuration;
using VetSolutionRation.wpf.Services.Feed;
using VetSolutionRation.wpf.Views.Import.Adapters;

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
    private readonly ObservableCollectionRanged<ImportedFileAdapter> _importedFiles;
    private readonly IConfigurationManager _configurationManager;
    public IDelegateCommandLight ImportCommand { get; }
    public IDelegateCommandLight<ImportedFileAdapter> DeleteFileCommand { get; }
    public ICollectionView ImportedFiles { get; }
    public IDelegateCommandLight OpenCacheFolderCommand { get; }

    public ImportViewModel(IFeedProvider feedProvider, IConfigurationManager configurationManager)
    {
        _feedProvider = feedProvider;
        _configurationManager = configurationManager;
        ImportCommand = new DelegateCommandLight(ExecuteImportCommand);
        DeleteFileCommand = new DelegateCommandLight<ImportedFileAdapter>(ExecuteDeleteFileCommand);
        ImportedFiles = ObservableCollectionSource.GetDefaultView(out _importedFiles);
        OpenCacheFolderCommand = new DelegateCommandLight(ExecuteOpenCacheFolderCommand);
    }

    private void ExecuteDeleteFileCommand(ImportedFileAdapter selectedImportedFile)
    {
        if (_importedFiles.Remove(selectedImportedFile))
        {
            _feedProvider.RemoveLabels(selectedImportedFile.FileFeedSource);
        }
    }

    private void ExecuteImportCommand()
    {
        var file = BrowserDialogManager.OpenFileBrowser("Fichiers (*.*)|*.*", null);
        if (file != null)
        {
            LoadInraeFile(file);
        }
    }    
    
    private void ExecuteOpenCacheFolderCommand()
    {
        AsyncWrapper.Wrap(() =>
        {
            _configurationManager.GetCacheDataFolder().OpenFolderInExplorer();
        });
    }

    private void LoadInraeFile(FileInfo file)
    {
        try
        {
            if (!FileFeedSourceExtensions.TryParseFromFileName(file.Name, out FileFeedSource feedSource))
            {
                MessageBox.Show($@"Le nom de fichier {file.Name} ne permet pas de lui attribuer une catégorie. il sera considéré comme un fichier de type 'Custom' par défaut");
            }

            IInraRationTableImportModel inraFile = InraRatioTableImporter.ImportInraTable(file);

            // remove other file of same type if they are not custom:
            RemoveExistingReferenceFile(feedSource);

            _importedFiles.Add(new ImportedFileAdapter(file.Name, feedSource));
            _feedProvider.LoadLabels(feedSource, inraFile.GetAllLabels());
        }
        catch (Exception e)
        {
            DebugCore.Fail($@"Erreur : {e}");
        }
    }

    private void RemoveExistingReferenceFile(FileFeedSource feedSource)
    {
        if (feedSource == FileFeedSource.Custom)
        {
            // for custom, don't remove duplicates file
            return;
        }
        var sameKindExistingFile = _importedFiles.FirstOrDefault(o => o.FileFeedSource == feedSource);
        if (sameKindExistingFile != null)
        {
            _importedFiles.Remove(sameKindExistingFile);
            _feedProvider.RemoveLabels(sameKindExistingFile.FileFeedSource);
        }
    }

    /// <inheritdoc />
    public void LoadDroppedFiles(FileInfo[] matchingFiles)
    {
        foreach (var file in matchingFiles)
        {
            LoadInraeFile(file);
        }
    }
}