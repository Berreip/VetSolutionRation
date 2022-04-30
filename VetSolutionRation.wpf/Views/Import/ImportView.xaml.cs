using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using VetSolutionRation.wpf.Helpers;
using VetSolutionRation.wpf.Services.Navigation;

namespace VetSolutionRation.wpf.Views.Import;

internal sealed partial class ImportView : INavigeablePanel
{
    private readonly IImportViewModel _vm;

    public ImportView(IImportViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
        _vm = vm;
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

  
    private void DropDiffFile(object sender, DragEventArgs e)
    {
        try
        {
            var files = e.Data.GetData(DataFormats.FileDrop, false) as string[];
            if (files == null || files.Length == 0)
            {
                return;
            }
               
            var matchingFiles = files.Select(o => new FileInfo(o)).Where(f => f.Exists && f.IsAllowedDiffExtension()).ToArray();
                
            if (matchingFiles.Length == 0)
            {
                MessageBox.Show(@"Aucun fichier n'est importable");
                return;
            }

            _vm.LoadDroppedFiles(matchingFiles);
        }
        catch (Exception exception)
        {
            Debug.Fail(exception.ToString());
        }
    }

    private void DragDiffEnter(object sender, DragEventArgs e)
    {
        if (!e.Data.GetDataPresent(DataFormats.FileDrop) || sender == e.Source)
        {
            e.Effects = DragDropEffects.None;
        }
        else
        {
            e.Effects = DragDropEffects.Copy;
        }
    }
}