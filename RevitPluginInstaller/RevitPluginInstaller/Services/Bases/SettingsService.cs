using RevitPluginInstaller.Services.Abstracts;
using RevitPluginInstaller.Models;
using Newtonsoft.Json; 
using System.IO;

namespace RevitPluginInstaller.Services.Bases;

public class SettingsService : ISettingsService
{
    private const string SettingsFileName = "C:\\Revit\\settings.json"; // @AppContext.BaseDirectory
    private SettingsResponse _settings;

    public SettingsService()
    {
        LoadSettingsAsync();
    }

    private async Task LoadSettingsAsync()
    {
        if (File.Exists(SettingsFileName))
        {
            var json = File.ReadAllText(SettingsFileName);
            _settings = JsonConvert.DeserializeObject<SettingsResponse>(json) ?? new();
        }
        else
        {
            _settings = new();
            await SaveSettingsAsync();
        }
    }

    private async Task SaveSettingsAsync()
    {
        var json = JsonConvert.SerializeObject(_settings);
        await File.WriteAllTextAsync(SettingsFileName, json);
    }

    public Task<string> GetRevitPathAsync()
    {
        return Task.FromResult(_settings.Settings.RevitPath);
    }

    public async Task SetRevitPathAsync(string path)
    {
        _settings.Settings.RevitPath = path;
        await SaveSettingsAsync();
    }

    public Task<string> GetSelectedVersionAsync()
    {
        return Task.FromResult(_settings.Settings.SelectedVersion);
    }

    public async Task SetSelectedVersionAsync(string version)
    {
        _settings.Settings.SelectedVersion = version;
        await SaveSettingsAsync();
    }
}