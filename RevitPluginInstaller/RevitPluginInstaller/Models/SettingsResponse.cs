using Newtonsoft.Json;

namespace RevitPluginInstaller.Models;

public class SettingsResponse
{
    [JsonProperty(nameof(Settings))]
    public Settings Settings { get; set; } = new();
}