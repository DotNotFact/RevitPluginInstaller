using RevitPluginInstaller.ViewModels.Pages;
using System.Windows.Controls;

namespace RevitPluginInstaller.Views.Pages;

public partial class UpdatePage : Page
{
    public UpdateViewModel ViewModel { get; }

    public UpdatePage(UpdateViewModel viewmodel)
    {
        ViewModel = viewmodel;
        DataContext = this;

        InitializeComponent();
    }
}