using RevitPluginInstaller.Infrastructure.Comands.Base;

namespace RevitPluginInstaller.Infrastructure.Comands;

public class NavigateCommand(Action<Type> navigate) : Command
{
    private readonly Action<Type> _navigate = navigate;

    public override bool CanExecute(object? parameter) => true;

    public override void Execute(object? parameter)
    {
        if (parameter is Type pageType)
        {
            _navigate(pageType);
        }
    }
}