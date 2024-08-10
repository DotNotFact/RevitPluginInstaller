namespace RevitPluginInstaller.Services.Abstracts;

public interface ISettingsService
{
    Task<string> GetRevitPathAsync();
    Task SetRevitPathAsync(string path);
    Task<string> GetSelectedVersionAsync();
    Task SetSelectedVersionAsync(string version);
}