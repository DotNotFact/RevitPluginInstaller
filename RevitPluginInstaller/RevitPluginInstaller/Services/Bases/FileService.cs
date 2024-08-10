using RevitPluginInstaller.Services.Abstracts;
using Microsoft.Win32;
using System.IO;

namespace RevitPluginInstaller.Services.Bases;

public class FileService : IFileService
{
    public Task<string> SelectFolderAsync()
    {
        var dialog = new OpenFolderDialog();

        if (dialog.ShowDialog() == true)
            return Task.FromResult(dialog.FolderName);

        return Task.FromResult(string.Empty);
    }

    public async Task CopyFileAsync(string source, string destination)
    {
        await Task.Run(() => File.Copy(source, destination, true));
    }

    public async Task CopyDirectoryAsync(string sourceDir, string destinationDir)
    {
        Directory.CreateDirectory(destinationDir);

        foreach (string file in Directory.GetFiles(sourceDir))
        {
            string destFile = Path.Combine(destinationDir, Path.GetFileName(file));
            await CopyFileAsync(file, destFile);
        }

        foreach (string dir in Directory.GetDirectories(sourceDir))
        {
            string destDir = Path.Combine(destinationDir, Path.GetFileName(dir));
            await CopyDirectoryAsync(dir, destDir);
        }
    }

    public async Task DeleteFileAsync(string path)
    {
        await Task.Run(() => File.Delete(path));
    }
}