using RevitPluginInstaller.Managers.Bases.Theme;
using RevitPluginInstaller.ViewModels.Windows;
using RevitPluginInstaller.Managers.Bases;
using System.Windows.Interop;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;

namespace RevitPluginInstaller.Views.Windows;

public partial class MainWindow : WindowThemeBase
{
    public MainWindowViewModel ViewModel { get; }

    public MainWindow(MainWindowViewModel viewModel)
    {
        //if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        //    return;

        InitializeComponent();

        DataContext = this;
        ViewModel = viewModel;
        ViewModel.SetMainFrame(MainFrame);
        MouseLeftButtonDown += OnMouseLeftButtonDown;
    }

    private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            WindowState = WindowState is WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }
        else if (e.ChangedButton == MouseButton.Left)
        {
            WindowsInteropAPI.SetPosition(new WindowInteropHelper(this).Handle, 0xA1, (IntPtr)0x2, IntPtr.Zero);
        }
    }
}