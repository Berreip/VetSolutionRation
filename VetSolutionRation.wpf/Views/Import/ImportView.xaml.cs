using System.Windows.Controls;
using VetSolutionRation.wpf.Services.Navigation;

namespace VetSolutionRation.wpf.Views.Import;

internal sealed partial class ImportView : INavigeablePanel
{
    public ImportView(IImportViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
    }

    /// <inheritdoc />
    public void OnNavigateTo()
    {
        // do nothing
    }

    /// <inheritdoc />
    public void OnNavigateExit()
    {
        // do nothing
    }
}