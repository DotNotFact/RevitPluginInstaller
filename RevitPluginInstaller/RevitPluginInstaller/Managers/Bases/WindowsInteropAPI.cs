using System.Runtime.InteropServices;

namespace RevitPluginInstaller.Managers.Bases;

public static class WindowsInteropAPI
{
    #region [ Theme ]

    private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

    [DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

    public static bool SetDarkMode(IntPtr hwnd, bool enable)
    {
        int darkMode = enable ? 1 : 0;
        return DwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE, ref darkMode, sizeof(int)) == 0;
    }

    #endregion

    #region [ DragMove ]

    [DllImport("user32.dll")]
    static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    static extern bool ReleaseCapture();

    public static IntPtr SetPosition(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam)
    {
        ReleaseCapture();
        return SendMessage(hWnd, msg, wParam, lParam);
    }

    #endregion
}