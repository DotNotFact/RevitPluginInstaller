using System.Windows.Interop;
using System.Windows;

namespace RevitPluginInstaller.Managers.Bases.Theme;

public abstract class WindowThemeBase : Window, IThemedWindow
{
    private static Style? defaultStyle = null;

    static WindowThemeBase()
    {
        StyleProperty.OverrideMetadata(typeof(WindowThemeBase), new FrameworkPropertyMetadata(GetDefautlStyle()));
    }

    private static Style? GetDefautlStyle()
    {
        defaultStyle ??= Application.Current.FindResource(typeof(WindowThemeBase)) as Style;
        return defaultStyle;
    }

    public void ApplyTheme(Theme theme)
    {
        if (PresentationSource.FromVisual(this) is HwndSource hwndSource)
        {
            WindowsInteropAPI.SetDarkMode(hwndSource.Handle, theme == Theme.Dark);

            // Обновляем ресурсы окна
            Resources.MergedDictionaries.Clear();
            Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = new Uri($"pack://application:,,,/RevitPluginInstaller;component/Resources/Themes/{theme}.xaml", UriKind.Absolute)
            });
        }
    }
}

