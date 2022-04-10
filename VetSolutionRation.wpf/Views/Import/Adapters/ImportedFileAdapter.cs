using PRF.WPFCore;
using VetSolutionRation.wpf.Helpers;

namespace VetSolutionRation.wpf.Views.Import.Adapters;

internal sealed class ImportedFileAdapter : ViewModelBase
{
    /// <inheritdoc />
    public ImportedFileAdapter(string importedFileName, FileFeedSource fileFeedSource)
    {
        ImportedFileName = importedFileName;
        FileFeedSource = fileFeedSource;
    }

    public string ImportedFileName { get; }
    public FileFeedSource FileFeedSource { get; }
}