using RevitPluginInstaller.ViewModels.Pages;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using RevitPluginInstaller.Models;

namespace RevitPluginInstaller.Views.Pages;

public partial class DownloadPage : Page
{
    public DownloadViewModel ViewModel { get; }

    public DownloadPage(DownloadViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }

    private void TreeView_PreviewDragOver(object sender, DragEventArgs e)
    {
        e.Handled = true;
        e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
    }

    private void TreeView_Drop(object sender, DragEventArgs e)
    {
        if (e.Data.GetDataPresent(DataFormats.FileDrop))
            ViewModel.DropCommand.Execute(e.Data);
    }

    private void TreeView_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        // Prevent the default behavior
        // e.Handled = true;
    }

    //private void ListBox_DragLeave(object sender, DragEventArgs e)
    //{
    //    e.Effects = DragDropEffects.None;
    //}
}
