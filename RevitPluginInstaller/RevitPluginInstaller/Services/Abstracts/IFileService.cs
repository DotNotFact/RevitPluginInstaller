namespace RevitPluginInstaller.Services.Abstracts;

public interface IFileService
{
    Task<string> SelectFolderAsync();
    Task CopyFileAsync(string source, string destination);
    Task CopyDirectoryAsync(string sourceDir, string destinationDir);
    Task DeleteFileAsync(string path);
}