using Newtonsoft.Json;
using RevitPluginInstaller.Models;
using RevitPluginInstaller.Services.Abstracts;
using System.IO;

namespace RevitPluginInstaller.Services.Bases;

public class SettingsService : ISettingsService
{
    private const string SettingsFileName = "settings.json"; 
    private string _settingsFileName;  
    private SettingsResponse _settings;

    public SettingsService()
    {
        LoadSettingsAsync();
    }

    private async Task LoadSettingsAsync()
    {
        _settingsFileName = AppContext.BaseDirectory + SettingsFileName;
        if (File.Exists(_settingsFileName))
        {
            var json = File.ReadAllText(_settingsFileName);
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
        await File.WriteAllTextAsync(_settingsFileName, json);
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