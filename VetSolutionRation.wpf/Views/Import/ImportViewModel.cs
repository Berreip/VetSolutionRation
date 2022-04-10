using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using PRF.WPFCore;
using PRF.WPFCore.Browsers;
using PRF.WPFCore.Commands;
using PRF.WPFCore.CustomCollections;
using VetSolutionRation.DataProvider;
using VetSolutionRation.wpf.Helpers;
using VetSolutionRation.wpf.Services.Feed;
using VetSolutionRation.wpf.Views.Import.Adapters;

namespace VetSolutionRation.wpf.Views.Import;

internal interface IImportViewModel
{
}

internal sealed class ImportViewModel : ViewModelBase, IImportViewModel
{
    private readonly IFeedProvider _feedProvider;
    private readonly ObservableCollectionRanged<ImportedFileAdapter> _importedFiles;
    public IDelegateCommandLight ImportCommand { get; }
    public IDelegateCommandLight<ImportedFileAdapter> DeleteFileCommand { get; }
    public ICollectionView ImportedFiles { get; }

    public ImportViewModel(IFeedProvider feedProvider)
    {
        _feedProvider = feedProvider;
        ImportCommand = new DelegateCommandLight(ExecuteImportCommand);
        DeleteFileCommand = new DelegateCommandLight<ImportedFileAdapter>(ExecuteDeleteFileCommand);
        ImportedFiles = ObservableCollectionSource.GetDefaultView(out _importedFiles);
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
        try
        {
            if (file != null)
            {
                if (!FileFeedSourceExtensions.TryParseFromFileName(file.Name, out var feedSource))
                {
                    MessageBox.Show(@"unable to parse file name to assign file type");
                }

                var inraFile = InraRatioTableImporter.ImportInraTable(file);

                // remove other file of same type:
                var sameKindExistingFile = _importedFiles.FirstOrDefault(o => o.FileFeedSource == feedSource);
                if (sameKindExistingFile != null)
                {
                    _importedFiles.Remove(sameKindExistingFile);
                    _feedProvider.RemoveLabels(sameKindExistingFile.FileFeedSource);
                }

                _importedFiles.Add(new ImportedFileAdapter(file.Name, feedSource));
                _feedProvider.LoadLabels(feedSource, inraFile.GetLabels());
            }
        }
        catch (Exception e)
        {
            MessageBox.Show($@"Exception : {e}");
        }
    }
}