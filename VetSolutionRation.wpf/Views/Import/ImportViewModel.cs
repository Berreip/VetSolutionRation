using System;
using System.Windows.Forms;
using PRF.WPFCore.Browsers;
using PRF.WPFCore.Commands;
using VetSolutionRation.DataProvider;
using VetSolutionRation.wpf.Helpers;
using VetSolutionRation.wpf.Services.Feed;

namespace VetSolutionRation.wpf.Views.Import;

internal interface IImportViewModel
{
}

internal sealed class ImportViewModel : IImportViewModel
{
    private readonly IFeedProvider _feedProvider;
    public IDelegateCommandLight ImportCommand { get; }

    public ImportViewModel(IFeedProvider feedProvider)
    {
        _feedProvider = feedProvider;
        ImportCommand = new DelegateCommandLight(ExecuteImportCommand);
    }

    private void ExecuteImportCommand()
    {
        var file = BrowserDialogManager.OpenFileBrowser("Fichiers (*.*)|*.*", null);
        try
        {
            if (file != null)
            {
                var inraFile =  InraRatioTableImporter.ImportInraTable(file);
                _feedProvider.LoadLabels(FileFeedSource.Concentrate, inraFile.GetLabels());
            }

        }
        catch (Exception e)
        {
            MessageBox.Show($@"Exception : {e}");
        }
    }
}