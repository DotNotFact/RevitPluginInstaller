using RevitPluginInstaller.ViewModels.Pages;
using System.Windows.Controls;

namespace RevitPluginInstaller.Views.Pages;

public partial class ChoosePage : Page
{
    public ChooseViewModel ViewModel { get; }

    public ChoosePage(ChooseViewModel viewModel)
    {
        ViewModel = viewModel;
        DataContext = this;

        InitializeComponent();
    }
}