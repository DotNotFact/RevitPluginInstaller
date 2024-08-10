using Newtonsoft.Json;

namespace RevitPluginInstaller.Models;
 
public class Settings
{
    [JsonProperty(nameof(RevitPath))]
    public string RevitPath { get; set; } = string.Empty;

    [JsonProperty(nameof(SelectedVersion))]
    public string SelectedVersion { get; set; } = string.Empty;
}