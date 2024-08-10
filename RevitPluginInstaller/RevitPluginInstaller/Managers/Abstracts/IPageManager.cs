using System.Windows.Controls;

namespace RevitPluginInstaller.Managers.Abstracts;

public interface IPageManager
{
    Page ActivePage { get; }

    void Initialization(Frame frame);
    void Navigate(Type pageType);
    void Navigate<T>() where T : class;
}