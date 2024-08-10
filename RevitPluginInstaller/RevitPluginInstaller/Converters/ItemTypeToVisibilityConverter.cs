using RevitPluginInstaller.ViewModels.Core;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace RevitPluginInstaller.Converters;

public class ItemTypeToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is TreeViewItemType itemType && parameter is string targetTypeText)
            return itemType.ToString() == targetTypeText ? Visibility.Visible : Visibility.Collapsed;

        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}