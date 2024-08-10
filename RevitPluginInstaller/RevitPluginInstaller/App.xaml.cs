using Microsoft.Extensions.DependencyInjection;
using RevitPluginInstaller.Views.Windows;
using RevitPluginInstaller.Extensions;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows;
using RevitPluginInstaller.Managers.Bases.Theme;

namespace RevitPluginInstaller;

public partial class App : Application
{
    public static IServiceProvider? ServiceProvider { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;

        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollection.AddPersistence();

        ServiceProvider = serviceCollection.BuildServiceProvider();

        var currentWindow = ServiceProvider.GetRequiredService<MainWindow>(); 
        ThemeManager.ApplyTheme(Theme.Dark);
        currentWindow.Show();

        currentWindow.Closed += (s, args) => { if (Current.Windows.Count == 0) Current.Shutdown(); };
    }
}