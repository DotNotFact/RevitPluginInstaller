using Newtonsoft.Json;

namespace RevitPluginInstaller.Models;

public class PluginPack
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public DateTime InstallationDate { get; set; }

    public List<Plugin> Plugins { get; set; } = []; 
} 