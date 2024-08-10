using RevitPluginInstaller.Infrastructure.Comands.Base;
using System.Windows;

namespace RevitPluginInstaller.Infrastructure.Comands;

public class CloseApplicationCommand : Command
{
    public override bool CanExecute(object? parameter) => true;

    public override void Execute(object? parameter)
    {
        Application.Current.Shutdown();
    }
}