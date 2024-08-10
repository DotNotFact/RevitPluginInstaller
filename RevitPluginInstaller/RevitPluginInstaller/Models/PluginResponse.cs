using Newtonsoft.Json;

namespace RevitPluginInstaller.Models;

public class PluginResponse
{   
    public List<PluginPack> PluginPacks { get; set; } = [];
}