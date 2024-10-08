﻿namespace RevitPluginInstaller.Models;

public class Plugin
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;

    public bool IsDrop { get; set; }
} 