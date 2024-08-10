using RevitPluginInstaller.ViewModels.Pages;
using System.Windows.Controls;

namespace RevitPluginInstaller.Views.Pages;

public partial class SettingPage : Page
{
    public SettingsViewModel ViewModel { get; }

    public SettingPage(SettingsViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }

}