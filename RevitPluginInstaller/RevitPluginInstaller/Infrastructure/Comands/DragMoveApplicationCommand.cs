using RevitPluginInstaller.Infrastructure.Comands.Base;
using System.Windows;

namespace RevitPluginInstaller.Infrastructure.Comands;

public class DragMoveApplicationCommand : Command
{
    public override bool CanExecute(object? parameter) => true;

    public override void Execute(object? parameter)
    {
        (parameter as Window)?.DragMove();
    }
}
