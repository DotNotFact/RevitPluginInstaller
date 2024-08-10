using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace RevitPluginInstaller.ViewModels.Base;

public abstract class ViewModel : INotifyPropertyChanged, IDisposable
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? PropertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
    }

    protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string? PropertyName = null)
    {
        if (Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(PropertyName);

        return true;
    }

    ~ViewModel()
    {
        Dispose(true);
    }

    public void Dispose()
    {
        Dispose(true);
    }

    private bool _disposed;
    public void Dispose(bool disposing)
    {
        if (!disposing || _disposed)
            return;

        _disposed = true;

        // Освобождает управляемые ресурсы
    }
}