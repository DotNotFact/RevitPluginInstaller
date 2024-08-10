using System.Windows;

namespace RevitPluginInstaller.Managers.Bases.Theme;

public static class ThemeManager
{
    private static ResourceDictionary? _currentTheme;

    public static void ApplyTheme(Theme theme)
    {
        var app = Application.Current;
        var mergedDicts = app.Resources.MergedDictionaries;

        // Удаляем текущую тему, если она есть
        if (_currentTheme is not null)
        {
            mergedDicts.Remove(_currentTheme);
        }

        // Загружаем и добавляем новую тему
        _currentTheme = new ResourceDictionary
        {
            Source = new Uri($"pack://application:,,,/RevitPluginInstaller;component/Resources/Themes/{theme}.xaml", UriKind.Absolute)
        };
        mergedDicts.Add(_currentTheme);

        // Применяем тему ко всем открытым окнам
        foreach (Window window in app.Windows)
            if (window is IThemedWindow themedWindow)
                themedWindow.ApplyTheme(theme);
    }
}