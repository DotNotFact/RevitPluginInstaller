using RevitPluginInstaller.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RevitPluginInstaller.ViewModels.Core;

public class TreeViewItemViewModel : ViewModel
{
    public string DisplayName { get; set; }

    public object Data { get; set; }
    public ObservableCollection<TreeViewItemViewModel> Children { get; } = [];

    public bool IsExpanded { get; set; }

    public TreeViewItemType ItemType { get; set; }

    public ICommand RemovePluginCommand { get; set; }
}