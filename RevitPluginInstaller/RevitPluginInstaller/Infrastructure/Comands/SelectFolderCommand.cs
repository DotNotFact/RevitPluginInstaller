using RevitPluginInstaller.Infrastructure.Comands.Base;

namespace RevitPluginInstaller.Infrastructure.Comands;

internal class SelectFolderCommand : Command
{
    public override bool CanExecute(object? parameter) => true;

    public override void Execute(object? parameter)
    {
    }
}
