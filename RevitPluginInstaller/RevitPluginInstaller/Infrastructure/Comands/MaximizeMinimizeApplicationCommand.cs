using RevitPluginInstaller.Infrastructure.Comands.Base;
using System.Windows;

namespace RevitPluginInstaller.Infrastructure.Comands;

internal class MaximizeMinimizeApplicationCommand : Command
{ 
    public override bool CanExecute(object? p) => true;

    public override void Execute(object? p)
    {
        if (p is Window window)
        {
            if (window.WindowState == WindowState.Maximized)
            {
                window.WindowState = WindowState.Normal;
            }
            else
            {
                window.WindowState = WindowState.Maximized;
            }
        }
    }
}