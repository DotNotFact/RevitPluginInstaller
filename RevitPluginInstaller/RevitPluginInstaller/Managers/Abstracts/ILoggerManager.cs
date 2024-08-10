namespace RevitPluginInstaller.Managers.Abstracts;

public interface ILoggerManager
{
    Task LogAsync(string message);
}