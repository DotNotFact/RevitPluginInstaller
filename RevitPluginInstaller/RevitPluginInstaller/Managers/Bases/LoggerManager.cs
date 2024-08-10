using RevitPluginInstaller.Managers.Abstracts;
using RevitPluginInstaller.Services.Abstracts;
using System.IO;

namespace RevitPluginInstaller.Managers.Bases;

public class LoggerManager(ISettingsService settingsService) : ILoggerManager
{
    private readonly ISettingsService _settingsService = settingsService;
    private const string LogFileName = "RevitPluginInstaller.log";

    public async Task LogAsync(string message)
    {
        var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
        File.AppendAllText(Path.Combine(await _settingsService.GetRevitPathAsync(), LogFileName), logMessage + Environment.NewLine);
    }
}