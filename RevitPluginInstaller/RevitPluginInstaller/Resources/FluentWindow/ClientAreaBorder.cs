//using System.ComponentModel;
//using System.Windows.Media;
//using System.Windows;
//using System.Runtime.InteropServices;
//using System.Drawing;
//using System.Windows.Interop;
//using Microsoft.Win32;
//using System.Windows.Shell;
//using System.Windows.Controls;

//namespace RevitPluginInstaller.Views.Resources.Window;

//public class ClientAreaBorder : System.Windows.Controls.Border, IThemeControl
//{
//    /*private const int SM_CXFRAME = 32;
//    private const int SM_CYFRAME = 33;
//    private const int SM_CXPADDEDBORDER = 92;*/
//    private static Thickness? _paddedBorderThickness;
//    private static Thickness? _resizeFrameBorderThickness;
//    private static Thickness? _windowChromeNonClientFrameThickness;
//    private bool _borderBrushApplied = false;
//    private System.Windows.Window? _oldWindow;

//    public ApplicationTheme ApplicationTheme { get; set; } = ApplicationTheme.Unknown;

//    /// <summary>
//    /// Gets the system value for the padded border thickness (<see cref="User32.SM.CXPADDEDBORDER"/>) in WPF units.
//    /// </summary>
//    public Thickness PaddedBorderThickness
//    {
//        get
//        {
//            if (_paddedBorderThickness is not null)
//            {
//                return _paddedBorderThickness.Value;
//            }

//            var paddedBorder = User32.GetSystemMetrics(User32.SM.CXPADDEDBORDER);

//            (double factorX, double factorY) = GetDpi();

//            var frameSize = new System.Windows.Size(paddedBorder, paddedBorder);
//            var frameSizeInDips = new System.Windows.Size(frameSize.Width / factorX, frameSize.Height / factorY);

//            _paddedBorderThickness = new Thickness(
//                frameSizeInDips.Width,
//                frameSizeInDips.Height,
//                frameSizeInDips.Width,
//                frameSizeInDips.Height
//            );

//            return _paddedBorderThickness.Value;
//        }
//    }

//    /// <summary>
//    /// Gets the system <see cref="User32.SM.CXFRAME"/> and <see cref="User32.SM.CYFRAME"/> values in WPF units.
//    /// </summary>
//    public static Thickness ResizeFrameBorderThickness =>
//        _resizeFrameBorderThickness ??= new Thickness(
//            SystemParameters.ResizeFrameVerticalBorderWidth,
//            SystemParameters.ResizeFrameHorizontalBorderHeight,
//            SystemParameters.ResizeFrameVerticalBorderWidth,
//            SystemParameters.ResizeFrameHorizontalBorderHeight
//        );

//    /// <summary>
//    /// Gets the thickness of the window's non-client frame used for maximizing the window with a custom chrome.
//    /// </summary>
//    /// <remarks>
//    /// If you use a <see cref="WindowChrome"/> to extend the client area of a window to the non-client area, you need to handle the edge margin issue when the window is maximized.
//    /// Use this property to get the correct margin value when the window is maximized, so that when the window is maximized, the client area can completely cover the screen client area by no less than a single pixel at any DPI.
//    /// The<see cref="User32.GetSystemMetrics"/> method cannot obtain this value directly.
//    /// </remarks>
//    public Thickness WindowChromeNonClientFrameThickness =>
//        _windowChromeNonClientFrameThickness ??= new Thickness(
//            ResizeFrameBorderThickness.Left + PaddedBorderThickness.Left,
//            ResizeFrameBorderThickness.Top + PaddedBorderThickness.Top,
//            ResizeFrameBorderThickness.Right + PaddedBorderThickness.Right,
//            ResizeFrameBorderThickness.Bottom + PaddedBorderThickness.Bottom
//        );

//    public ClientAreaBorder()
//    {
//        //ApplicationTheme = ApplicationThemeManager.GetAppTheme();
//        //ApplicationThemeManager.Changed += OnThemeChanged;
//    }

//    private void OnThemeChanged(ApplicationTheme currentApplicationTheme, System.Drawing.Color systemAccent)
//    {
//        ApplicationTheme = currentApplicationTheme;

//        if (!_borderBrushApplied || _oldWindow == null)
//        {
//            return;
//        }

//        ApplyDefaultWindowBorder();
//    }

//    /// <inheritdoc />
//    protected override void OnVisualParentChanged(DependencyObject oldParent)
//    {
//        base.OnVisualParentChanged(oldParent);

//        if (_oldWindow is { } oldWindow)
//        {
//            oldWindow.StateChanged -= OnWindowStateChanged;
//            oldWindow.Closing -= OnWindowClosing;
//        }

//        var newWindow = (System.Windows.Window?)System.Windows.Window.GetWindow(this);

//        if (newWindow is not null)
//        {
//            newWindow.StateChanged -= OnWindowStateChanged; // Unsafe
//            newWindow.StateChanged += OnWindowStateChanged;
//            newWindow.Closing += OnWindowClosing;
//        }

//        _oldWindow = newWindow;

//        ApplyDefaultWindowBorder();
//    }

//    private void OnWindowClosing(object? sender, CancelEventArgs e)
//    {
//        //ApplicationThemeManager.Changed -= OnThemeChanged;

//        if (_oldWindow != null)
//        {
//            _oldWindow.Closing -= OnWindowClosing;
//        }
//    }

//    private void OnWindowStateChanged(object? sender, EventArgs e)
//    {
//        if (sender is not System.Windows.Window window)
//        {
//            return;
//        }

//        Thickness padding = window.WindowState == WindowState.Maximized ? WindowChromeNonClientFrameThickness : default;
//        SetCurrentValue(PaddingProperty, padding);
//    }

//    private void ApplyDefaultWindowBorder()
//    {
//        //if (Utilities.IsOSWindows11OrNewer || _oldWindow == null)
//        //{
//        //    return;
//        //}

//        _borderBrushApplied = true;

//        // SystemParameters.WindowGlassBrush
//        System.Windows.Media.Color borderColor =
//            ApplicationTheme == ApplicationTheme.Light
//                ? System.Windows.Media.Color.FromArgb(0xFF, 0x7A, 0x7A, 0x7A)
//                : System.Windows.Media.Color.FromArgb(0xFF, 0x3A, 0x3A, 0x3A);

//        _oldWindow.SetCurrentValue(
//            System.Windows.Controls.Control.BorderBrushProperty,
//            new SolidColorBrush(borderColor)
//        );
//        _oldWindow.SetCurrentValue(System.Windows.Controls.Control.BorderThicknessProperty, new Thickness(1));
//    }

//    private (double FactorX, double FactorY) GetDpi()
//    {
//        if (PresentationSource.FromVisual(this) is { } source)
//        {
//            return (
//                source.CompositionTarget.TransformToDevice.M11, // Possible null reference
//                source.CompositionTarget.TransformToDevice.M22
//            );
//        }

//        DisplayDpi systemDPi = DpiHelper.GetSystemDpi();

//        return (systemDPi.DpiScaleX, systemDPi.DpiScaleY);
//    }
//}

//public interface IThemeControl
//{
//    /// <summary>
//    /// Gets the theme that is currently set.
//    /// </summary>
//    public ApplicationTheme ApplicationTheme { get; }
//}

//public enum ApplicationTheme
//{
//    /// <summary>
//    /// Unknown application theme.
//    /// </summary>
//    Unknown,

//    /// <summary>
//    /// Dark application theme.
//    /// </summary>
//    Dark,

//    /// <summary>
//    /// Light application theme.
//    /// </summary>
//    Light,

//    /// <summary>
//    /// High contract application theme.
//    /// </summary>
//    HighContrast
//}


//// This Source Code Form is subject to the terms of the MIT License.
//// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
//// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
//// All Rights Reserved.

///* This Source Code is partially based on reverse engineering of the Windows Operating System,
//   and is intended for use on Windows systems only.
//   This Source Code is partially based on the source code provided by the .NET Foundation.

//   NOTE:
//   I split unmanaged code stuff into the NativeMethods library.
//   If you have suggestions for the code below, please submit your changes there.
//   https://github.com/lepoco/nativemethods */

//// ReSharper disable IdentifierTypo
//// ReSharper disable InconsistentNaming
//#pragma warning disable SA1300 // Element should begin with upper-case letter
//#pragma warning disable SA1307 // Accessible fields should begin with upper-case letter
//#pragma warning disable SA1401 // Fields should be private

///// <summary>
///// USER procedure declarations, constant definitions and macros.
///// </summary>
//internal static class User32
//{
//    /// <summary>
//    /// SetWindowPos options
//    /// </summary>
//    [Flags]
//    public enum SWP
//    {
//        ASYNCWINDOWPOS = 0x4000,
//        DEFERERASE = 0x2000,
//        DRAWFRAME = 0x0020,
//        FRAMECHANGED = DRAWFRAME,
//        HIDEWINDOW = 0x0080,
//        NOACTIVATE = 0x0010,
//        NOCOPYBITS = 0x0100,
//        NOMOVE = 0x0002,
//        NOOWNERZORDER = 0x0200,
//        NOREDRAW = 0x0008,
//        NOREPOSITION = NOOWNERZORDER,
//        NOSENDCHANGING = 0x0400,
//        NOSIZE = 0x0001,
//        NOZORDER = 0x0004,
//        SHOWWINDOW = 0x0040,
//    }

//    /// <summary>
//    /// EnableMenuItem uEnable values, MF_*
//    /// </summary>
//    [Flags]
//    public enum MF : uint
//    {
//        /// <summary>
//        /// Possible return value for EnableMenuItem
//        /// </summary>
//        DOES_NOT_EXIST = unchecked((uint)-1),
//        ENABLED = 0,
//        BYCOMMAND = ENABLED,
//        GRAYED = 1,
//        DISABLED = 2,
//    }

//    /// <summary>
//    /// Menu item element.
//    /// </summary>
//    public enum SC
//    {
//        SIZE = 0xF000,
//        MOVE = 0xF010,
//        MINIMIZE = 0xF020,
//        MAXIMIZE = 0xF030,
//        NEXTWINDOW = 0xF040,
//        PREVWINDOW = 0xF050,
//        CLOSE = 0xF060,
//        VSCROLL = 0xF070,
//        HSCROLL = 0xF080,
//        MOUSEMENU = 0xF090,
//        KEYMENU = 0xF100,
//        ARRANGE = 0xF110,
//        RESTORE = 0xF120,
//        TASKLIST = 0xF130,
//        SCREENSAVE = 0xF140,
//        HOTKEY = 0xF150,
//        DEFAULT = 0xF160,
//        MONITORPOWER = 0xF170,
//        CONTEXTHELP = 0xF180,
//        SEPARATOR = 0xF00F,

//        /// <summary>
//        /// SCF_ISSECURE
//        /// </summary>
//        F_ISSECURE = 0x00000001,
//        ICON = MINIMIZE,
//        ZOOM = MAXIMIZE,
//    }

//    /// <summary>
//    /// WM_NCHITTEST and MOUSEHOOKSTRUCT Mouse Position Codes
//    /// </summary>
//    public enum WM_NCHITTEST
//    {
//        /// <summary>
//        /// Hit test returned error.
//        /// </summary>
//        HTERROR = unchecked(-2),

//        /// <summary>
//        /// Hit test returned transparent.
//        /// </summary>
//        HTTRANSPARENT = unchecked(-1),

//        /// <summary>
//        /// On the screen background or on a dividing line between windows.
//        /// </summary>
//        HTNOWHERE = 0,

//        /// <summary>
//        /// In a client area.
//        /// </summary>
//        HTCLIENT = 1,

//        /// <summary>
//        /// In a title bar.
//        /// </summary>
//        HTCAPTION = 2,

//        /// <summary>
//        /// In a window menu or in a Close button in a child window.
//        /// </summary>
//        HTSYSMENU = 3,

//        /// <summary>
//        /// In a size box (same as HTSIZE).
//        /// </summary>
//        HTGROWBOX = 4,
//        HTSIZE = HTGROWBOX,

//        /// <summary>
//        /// In a menu.
//        /// </summary>
//        HTMENU = 5,

//        /// <summary>
//        /// In a horizontal scroll bar.
//        /// </summary>
//        HTHSCROLL = 6,

//        /// <summary>
//        /// In the vertical scroll bar.
//        /// </summary>
//        HTVSCROLL = 7,

//        /// <summary>
//        /// In a Minimize button.
//        /// </summary>
//        HTMINBUTTON = 8,

//        /// <summary>
//        /// In a Maximize button.
//        /// </summary>
//        HTMAXBUTTON = 9,

//        // ZOOM = 9,

//        /// <summary>
//        /// In the left border of a resizable window (the user can click the mouse to resize the window horizontally).
//        /// </summary>
//        HTLEFT = 10,

//        /// <summary>
//        /// In the right border of a resizable window (the user can click the mouse to resize the window horizontally).
//        /// </summary>
//        HTRIGHT = 11,

//        /// <summary>
//        /// In the upper-horizontal border of a window.
//        /// </summary>
//        HTTOP = 12,

//        // From 10.0.22000.0\um\WinUser.h
//        HTTOPLEFT = 13,
//        HTTOPRIGHT = 14,
//        HTBOTTOM = 15,
//        HTBOTTOMLEFT = 16,
//        HTBOTTOMRIGHT = 17,
//        HTBORDER = 18,
//        HTREDUCE = HTMINBUTTON,
//        HTZOOM = HTMAXBUTTON,
//        HTSIZEFIRST = HTLEFT,
//        HTSIZELAST = HTBOTTOMRIGHT,
//        HTOBJECT = 19,
//        HTCLOSE = 20,
//        HTHELP = 21
//    }

//    /// <summary>
//    /// Window long flags.
//    /// <para><see href="https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setwindowlonga"/></para>
//    /// </summary>
//    [Flags]
//    public enum GWL
//    {
//        /// <summary>
//        /// Sets a new extended window style.
//        /// </summary>
//        GWL_EXSTYLE = -20,

//        /// <summary>
//        /// Sets a new application instance handle.
//        /// </summary>
//        GWLP_HINSTANCE = -6,

//        /// <summary>
//        /// Sets a new hWnd parent.
//        /// </summary>
//        GWLP_HWNDPARENT = -8,

//        /// <summary>
//        /// Sets a new identifier of the child window. The window cannot be a top-level window.
//        /// </summary>
//        GWL_ID = -12,

//        /// <summary>
//        /// Sets a new window style.
//        /// </summary>
//        GWL_STYLE = -16,

//        /// <summary>
//        /// Sets the user data associated with the window.
//        /// This data is intended for use by the application that created the window. Its value is initially zero.
//        /// </summary>
//        GWL_USERDATA = -21,

//        /// <summary>
//        /// Sets a new address for the window procedure.
//        /// You cannot change this attribute if the window does not belong to the same process as the calling thread.
//        /// </summary>
//        GWL_WNDPROC = -4,

//        /// <summary>
//        /// Sets new extra information that is private to the application, such as handles or pointers.
//        /// </summary>
//        DWLP_USER = 0x8,

//        /// <summary>
//        /// Sets the return value of a message processed in the dialog box procedure.
//        /// </summary>
//        DWLP_MSGRESULT = 0x0,

//        /// <summary>
//        /// Sets the new address of the dialog box procedure.
//        /// </summary>
//        DWLP_DLGPROC = 0x4
//    }

//    /// <summary>
//    /// Window composition attributes.
//    /// </summary>
//    public enum WCA
//    {
//        WCA_UNDEFINED = 0,
//        WCA_NCRENDERING_ENABLED = 1,
//        WCA_NCRENDERING_POLICY = 2,
//        WCA_TRANSITIONS_FORCEDISABLED = 3,
//        WCA_ALLOW_NCPAINT = 4,
//        WCA_CAPTION_BUTTON_BOUNDS = 5,
//        WCA_NONCLIENT_RTL_LAYOUT = 6,
//        WCA_FORCE_ICONIC_REPRESENTATION = 7,
//        WCA_EXTENDED_FRAME_BOUNDS = 8,
//        WCA_HAS_ICONIC_BITMAP = 9,
//        WCA_THEME_ATTRIBUTES = 10,
//        WCA_NCRENDERING_EXILED = 11,
//        WCA_NCADORNMENTINFO = 12,
//        WCA_EXCLUDED_FROM_LIVEPREVIEW = 13,
//        WCA_VIDEO_OVERLAY_ACTIVE = 14,
//        WCA_FORCE_ACTIVEWINDOW_APPEARANCE = 15,
//        WCA_DISALLOW_PEEK = 16,
//        WCA_CLOAK = 17,
//        WCA_CLOAKED = 18,
//        WCA_ACCENT_POLICY = 19,
//        WCA_FREEZE_REPRESENTATION = 20,
//        WCA_EVER_UNCLOAKED = 21,
//        WCA_VISUAL_OWNER = 22,
//        WCA_HOLOGRAPHIC = 23,
//        WCA_EXCLUDED_FROM_DDA = 24,
//        WCA_PASSIVEUPDATEMODE = 25,
//        WCA_USEDARKMODECOLORS = 26,
//        WCA_CORNER_STYLE = 27,
//        WCA_PART_COLOR = 28,
//        WCA_DISABLE_MOVESIZE_FEEDBACK = 29,
//        WCA_LAST = 30
//    }

//    [Flags]
//    public enum ACCENT_FLAGS
//    {
//        DrawLeftBorder = 0x20,
//        DrawTopBorder = 0x40,
//        DrawRightBorder = 0x80,
//        DrawBottomBorder = 0x100,
//        DrawAllBorders = DrawLeftBorder | DrawTopBorder | DrawRightBorder | DrawBottomBorder
//    }

//    /// <summary>
//    /// DWM window accent state.
//    /// </summary>
//    public enum ACCENT_STATE
//    {
//        ACCENT_DISABLED = 0,
//        ACCENT_ENABLE_GRADIENT = 1,
//        ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
//        ACCENT_ENABLE_BLURBEHIND = 3,
//        ACCENT_ENABLE_ACRYLICBLURBEHIND = 4,
//        ACCENT_INVALID_STATE = 5
//    }

//    /// <summary>
//    /// WCA window accent policy.
//    /// </summary>
//    [StructLayout(LayoutKind.Sequential)]
//    public struct ACCENT_POLICY
//    {
//        public ACCENT_STATE nAccentState;
//        public uint nFlags;
//        public uint nColor;
//        public uint nAnimationId;
//    }

//    [StructLayout(LayoutKind.Sequential)]
//    public struct WINCOMPATTRDATA
//    {
//        public WCA Attribute;
//        public IntPtr Data;
//        public int SizeOfData;
//    }

//    /// <summary>
//    /// CS_*
//    /// </summary>
//    [Flags]
//    public enum CS : uint
//    {
//        VREDRAW = 0x0001,
//        HREDRAW = 0x0002,
//        DBLCLKS = 0x0008,
//        OWNDC = 0x0020,
//        CLASSDC = 0x0040,
//        PARENTDC = 0x0080,
//        NOCLOSE = 0x0200,
//        SAVEBITS = 0x0800,
//        BYTEALIGNCLIENT = 0x1000,
//        BYTEALIGNWINDOW = 0x2000,
//        GLOBALCLASS = 0x4000,
//        IME = 0x00010000,
//        DROPSHADOW = 0x00020000
//    }

//    /// <summary>
//    /// MSGFLT_*. New in Vista. Realiased in Windows 7.
//    /// </summary>
//    public enum MSGFLT
//    {
//        // Win7 versions of this enum:

//        /// <summary>
//        /// Resets the window message filter for hWnd to the default. Any message allowed globally or process-wide will get through, but any message not included in those two categories, and which comes from a lower privileged process, will be blocked.
//        /// </summary>
//        RESET = 0,

//        /// <summary>
//        /// Allows the message through the filter. This enables the message to be received by hWnd, regardless of the source of the message, even it comes from a lower privileged process.
//        /// </summary>
//        ALLOW = 1,

//        /// <summary>
//        /// Blocks the message to be delivered to hWnd if it comes from a lower privileged process, unless the message is allowed process-wide by using the ChangeWindowMessageFilter function or globally.
//        /// </summary>
//        DISALLOW = 2,

//        // Vista versions of this enum:
//        // ADD = 1,
//        // REMOVE = 2,
//    }

//    /// <summary>
//    /// MSGFLTINFO.
//    /// </summary>
//    public enum MSGFLTINFO
//    {
//        NONE = 0,
//        ALREADYALLOWED_FORWND = 1,
//        ALREADYDISALLOWED_FORWND = 2,
//        ALLOWED_HIGHER = 3,
//    }

//    /// <summary>
//    /// Win7 only.
//    /// </summary>
//    [StructLayout(LayoutKind.Sequential)]
//    public struct CHANGEFILTERSTRUCT
//    {
//        public uint cbSize;
//        public MSGFLTINFO ExtStatus;
//    }

//    /// <summary>
//    /// Window message values, WM_*
//    /// </summary>
//    public enum WM
//    {
//        NULL = 0x0000,
//        CREATE = 0x0001,
//        DESTROY = 0x0002,
//        MOVE = 0x0003,
//        SIZE = 0x0005,
//        ACTIVATE = 0x0006,
//        SETFOCUS = 0x0007,
//        KILLFOCUS = 0x0008,
//        ENABLE = 0x000A,
//        SETREDRAW = 0x000B,
//        SETTEXT = 0x000C,
//        GETTEXT = 0x000D,
//        GETTEXTLENGTH = 0x000E,
//        PAINT = 0x000F,
//        CLOSE = 0x0010,
//        QUERYENDSESSION = 0x0011,
//        QUIT = 0x0012,
//        QUERYOPEN = 0x0013,
//        ERASEBKGND = 0x0014,
//        SYSCOLORCHANGE = 0x0015,
//        SHOWWINDOW = 0x0018,
//        CTLCOLOR = 0x0019,
//        WININICHANGE = 0x001A,
//        SETTINGCHANGE = WININICHANGE,
//        ACTIVATEAPP = 0x001C,
//        SETCURSOR = 0x0020,
//        MOUSEACTIVATE = 0x0021,
//        CHILDACTIVATE = 0x0022,
//        QUEUESYNC = 0x0023,
//        GETMINMAXINFO = 0x0024,

//        WINDOWPOSCHANGING = 0x0046,
//        WINDOWPOSCHANGED = 0x0047,

//        CONTEXTMENU = 0x007B,
//        STYLECHANGING = 0x007C,
//        STYLECHANGED = 0x007D,
//        DISPLAYCHANGE = 0x007E,
//        GETICON = 0x007F,
//        SETICON = 0x0080,
//        NCCREATE = 0x0081,
//        NCDESTROY = 0x0082,
//        NCCALCSIZE = 0x0083,
//        NCHITTEST = 0x0084,
//        NCPAINT = 0x0085,
//        NCACTIVATE = 0x0086,
//        GETDLGCODE = 0x0087,
//        SYNCPAINT = 0x0088,
//        NCMOUSEMOVE = 0x00A0,
//        NCLBUTTONDOWN = 0x00A1,
//        NCLBUTTONUP = 0x00A2,
//        NCLBUTTONDBLCLK = 0x00A3,
//        NCRBUTTONDOWN = 0x00A4,
//        NCRBUTTONUP = 0x00A5,
//        NCRBUTTONDBLCLK = 0x00A6,
//        NCMBUTTONDOWN = 0x00A7,
//        NCMBUTTONUP = 0x00A8,
//        NCMBUTTONDBLCLK = 0x00A9,

//        SYSKEYDOWN = 0x0104,
//        SYSKEYUP = 0x0105,
//        SYSCHAR = 0x0106,
//        SYSDEADCHAR = 0x0107,
//        COMMAND = 0x0111,
//        SYSCOMMAND = 0x0112,

//        MOUSEMOVE = 0x0200,
//        LBUTTONDOWN = 0x0201,
//        LBUTTONUP = 0x0202,
//        LBUTTONDBLCLK = 0x0203,
//        RBUTTONDOWN = 0x0204,
//        RBUTTONUP = 0x0205,
//        RBUTTONDBLCLK = 0x0206,
//        MBUTTONDOWN = 0x0207,
//        MBUTTONUP = 0x0208,
//        MBUTTONDBLCLK = 0x0209,
//        MOUSEWHEEL = 0x020A,
//        XBUTTONDOWN = 0x020B,
//        XBUTTONUP = 0x020C,
//        XBUTTONDBLCLK = 0x020D,
//        MOUSEHWHEEL = 0x020E,
//        PARENTNOTIFY = 0x0210,

//        CAPTURECHANGED = 0x0215,
//        POWERBROADCAST = 0x0218,
//        DEVICECHANGE = 0x0219,

//        ENTERSIZEMOVE = 0x0231,
//        EXITSIZEMOVE = 0x0232,

//        IME_SETCONTEXT = 0x0281,
//        IME_NOTIFY = 0x0282,
//        IME_CONTROL = 0x0283,
//        IME_COMPOSITIONFULL = 0x0284,
//        IME_SELECT = 0x0285,
//        IME_CHAR = 0x0286,
//        IME_REQUEST = 0x0288,
//        IME_KEYDOWN = 0x0290,
//        IME_KEYUP = 0x0291,

//        NCMOUSELEAVE = 0x02A2,

//        TABLET_DEFBASE = 0x02C0,

//        // WM_TABLET_MAXOFFSET = 0x20,
//        TABLET_ADDED = TABLET_DEFBASE + 8,
//        TABLET_DELETED = TABLET_DEFBASE + 9,
//        TABLET_FLICK = TABLET_DEFBASE + 11,
//        TABLET_QUERYSYSTEMGESTURESTATUS = TABLET_DEFBASE + 12,

//        CUT = 0x0300,
//        COPY = 0x0301,
//        PASTE = 0x0302,
//        CLEAR = 0x0303,
//        UNDO = 0x0304,
//        RENDERFORMAT = 0x0305,
//        RENDERALLFORMATS = 0x0306,
//        DESTROYCLIPBOARD = 0x0307,
//        DRAWCLIPBOARD = 0x0308,
//        PAINTCLIPBOARD = 0x0309,
//        VSCROLLCLIPBOARD = 0x030A,
//        SIZECLIPBOARD = 0x030B,
//        ASKCBFORMATNAME = 0x030C,
//        CHANGECBCHAIN = 0x030D,
//        HSCROLLCLIPBOARD = 0x030E,
//        QUERYNEWPALETTE = 0x030F,
//        PALETTEISCHANGING = 0x0310,
//        PALETTECHANGED = 0x0311,
//        HOTKEY = 0x0312,
//        PRINT = 0x0317,
//        PRINTCLIENT = 0x0318,
//        APPCOMMAND = 0x0319,
//        THEMECHANGED = 0x031A,

//        DWMCOMPOSITIONCHANGED = 0x031E,
//        DWMNCRENDERINGCHANGED = 0x031F,
//        DWMCOLORIZATIONCOLORCHANGED = 0x0320,
//        DWMWINDOWMAXIMIZEDCHANGE = 0x0321,

//        GETTITLEBARINFOEX = 0x033F,

//        DWMSENDICONICTHUMBNAIL = 0x0323,
//        DWMSENDICONICLIVEPREVIEWBITMAP = 0x0326,

//        USER = 0x0400,

//        /// <summary>
//        /// This is the hard-coded message value used by WinForms for Shell_NotifyIcon.
//        /// It's relatively safe to reuse.
//        /// </summary>
//        TRAYMOUSEMESSAGE = 0x800, // WM_USER + 1024
//        APP = 0x8000,
//    }

//    /// <summary>
//    /// WindowStyle values, WS_*
//    /// </summary>
//    [Flags]
//    public enum WS : long
//    {
//        OVERLAPPED = 0x00000000,
//        POPUP = 0x80000000,
//        CHILD = 0x40000000,
//        MINIMIZE = 0x20000000,
//        VISIBLE = 0x10000000,
//        DISABLED = 0x08000000,
//        CLIPSIBLINGS = 0x04000000,
//        CLIPCHILDREN = 0x02000000,
//        MAXIMIZE = 0x01000000,
//        BORDER = 0x00800000,
//        DLGFRAME = 0x00400000,
//        VSCROLL = 0x00200000,
//        HSCROLL = 0x00100000,
//        SYSMENU = 0x00080000,
//        THICKFRAME = 0x00040000,
//        GROUP = 0x00020000,
//        TABSTOP = 0x00010000,

//        MINIMIZEBOX = GROUP,
//        MAXIMIZEBOX = TABSTOP,

//        CAPTION = BORDER | DLGFRAME,
//        TILED = OVERLAPPED,
//        ICONIC = MINIMIZE,
//        SIZEBOX = THICKFRAME,

//        OVERLAPPEDWINDOW = OVERLAPPED | CAPTION | SYSMENU | THICKFRAME | MINIMIZEBOX | MAXIMIZEBOX,
//        TILEDWINDOW = OVERLAPPEDWINDOW,

//        POPUPWINDOW = POPUP | BORDER | SYSMENU,
//        CHILDWINDOW = CHILD,
//    }

//    /// <summary>
//    /// Window style extended values, WS_EX_*
//    /// </summary>
//    [Flags]
//    public enum WS_EX : long
//    {
//        NONE = 0x00000000,
//        DLGMODALFRAME = 0x00000001,
//        NOPARENTNOTIFY = 0x00000004,
//        TOPMOST = 0x00000008,
//        ACCEPTFILES = 0x00000010,
//        TRANSPARENT = 0x00000020,
//        MDICHILD = 0x00000040,
//        TOOLWINDOW = 0x00000080,
//        WINDOWEDGE = 0x00000100,
//        CLIENTEDGE = 0x00000200,
//        CONTEXTHELP = 0x00000400,
//        RIGHT = 0x00001000,
//        LEFT = NONE,
//        RTLREADING = 0x00002000,
//        LTRREADING = NONE,
//        LEFTSCROLLBAR = 0x00004000,
//        RIGHTSCROLLBAR = NONE,
//        CONTROLPARENT = 0x00010000,
//        STATICEDGE = 0x00020000,
//        APPWINDOW = 0x00040000,
//        LAYERED = 0x00080000,
//        NOINHERITLAYOUT = 0x00100000, // Disable inheritence of mirroring by children
//        LAYOUTRTL = 0x00400000, // Right to left mirroring
//        COMPOSITED = 0x02000000,
//        NOACTIVATE = 0x08000000,
//        OVERLAPPEDWINDOW = WINDOWEDGE | CLIENTEDGE,
//        PALETTEWINDOW = WINDOWEDGE | TOOLWINDOW | TOPMOST,
//    }

//    /// <summary>
//    /// SystemMetrics.  SM_*
//    /// </summary>
//    public enum SM
//    {
//        CXSCREEN = 0,
//        CYSCREEN = 1,
//        CXVSCROLL = 2,
//        CYHSCROLL = 3,
//        CYCAPTION = 4,
//        CXBORDER = 5,
//        CYBORDER = 6,
//        CXFIXEDFRAME = 7,
//        CYFIXEDFRAME = 8,
//        CYVTHUMB = 9,
//        CXHTHUMB = 10,
//        CXICON = 11,
//        CYICON = 12,
//        CXCURSOR = 13,
//        CYCURSOR = 14,
//        CYMENU = 15,
//        CXFULLSCREEN = 16,
//        CYFULLSCREEN = 17,
//        CYKANJIWINDOW = 18,
//        MOUSEPRESENT = 19,
//        CYVSCROLL = 20,
//        CXHSCROLL = 21,
//        DEBUG = 22,
//        SWAPBUTTON = 23,
//        CXMIN = 28,
//        CYMIN = 29,
//        CXSIZE = 30,
//        CYSIZE = 31,
//        CXFRAME = 32,
//        CXSIZEFRAME = CXFRAME,
//        CYFRAME = 33,
//        CYSIZEFRAME = CYFRAME,
//        CXMINTRACK = 34,
//        CYMINTRACK = 35,
//        CXDOUBLECLK = 36,
//        CYDOUBLECLK = 37,
//        CXICONSPACING = 38,
//        CYICONSPACING = 39,
//        MENUDROPALIGNMENT = 40,
//        PENWINDOWS = 41,
//        DBCSENABLED = 42,
//        CMOUSEBUTTONS = 43,
//        SECURE = 44,
//        CXEDGE = 45,
//        CYEDGE = 46,
//        CXMINSPACING = 47,
//        CYMINSPACING = 48,
//        CXSMICON = 49,
//        CYSMICON = 50,
//        CYSMCAPTION = 51,
//        CXSMSIZE = 52,
//        CYSMSIZE = 53,
//        CXMENUSIZE = 54,
//        CYMENUSIZE = 55,
//        ARRANGE = 56,
//        CXMINIMIZED = 57,
//        CYMINIMIZED = 58,
//        CXMAXTRACK = 59,
//        CYMAXTRACK = 60,
//        CXMAXIMIZED = 61,
//        CYMAXIMIZED = 62,
//        NETWORK = 63,
//        CLEANBOOT = 67,
//        CXDRAG = 68,
//        CYDRAG = 69,
//        SHOWSOUNDS = 70,
//        CXMENUCHECK = 71,
//        CYMENUCHECK = 72,
//        SLOWMACHINE = 73,
//        MIDEASTENABLED = 74,
//        MOUSEWHEELPRESENT = 75,
//        XVIRTUALSCREEN = 76,
//        YVIRTUALSCREEN = 77,
//        CXVIRTUALSCREEN = 78,
//        CYVIRTUALSCREEN = 79,
//        CMONITORS = 80,
//        SAMEDISPLAYFORMAT = 81,
//        IMMENABLED = 82,
//        CXFOCUSBORDER = 83,
//        CYFOCUSBORDER = 84,
//        TABLETPC = 86,
//        MEDIACENTER = 87,
//        CXPADDEDBORDER = 92,
//        REMOTESESSION = 0x1000,
//        REMOTECONTROL = 0x2001,
//    }

//    /// <summary>
//    /// ShowWindow options
//    /// </summary>
//    public enum SW
//    {
//        HIDE = 0,
//        SHOWNORMAL = 1,
//        NORMAL = SHOWNORMAL,
//        SHOWMINIMIZED = 2,
//        SHOWMAXIMIZED = 3,
//        MAXIMIZE = SHOWMAXIMIZED,
//        SHOWNOACTIVATE = 4,
//        SHOW = 5,
//        MINIMIZE = 6,
//        SHOWMINNOACTIVE = 7,
//        SHOWNA = 8,
//        RESTORE = 9,
//        SHOWDEFAULT = 10,
//        FORCEMINIMIZE = 11,
//    }

//    [StructLayout(LayoutKind.Sequential)]
//    public class WINDOWPLACEMENT
//    {
//        public int length = Marshal.SizeOf(typeof(WINDOWPLACEMENT));
//        public int flags;
//        public SW showCmd;
//        public POINT ptMinPosition;
//        public POINT ptMaxPosition;
//        public RECT rcNormalPosition;
//    }

//    /// <summary>
//    /// Contains window class information. It is used with the <see cref="RegisterClassEx"/> and GetClassInfoEx functions.
//    /// </summary>
//    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
//    public struct WNDCLASSEX
//    {
//        /// <summary>
//        /// The size, in bytes, of this structure. Set this member to sizeof(WNDCLASSEX). Be sure to set this member before calling the GetClassInfoEx function.
//        /// </summary>
//        public int cbSize;

//        /// <summary>
//        /// The class style(s). This member can be any combination of the Class Styles.
//        /// </summary>
//        public CS style;

//        /// <summary>
//        /// A pointer to the window procedure. You must use the CallWindowProc function to call the window procedure. For more information, see WindowProc.
//        /// </summary>
//        public WndProc lpfnWndProc;

//        /// <summary>
//        /// The number of extra bytes to allocate following the window-class structure. The system initializes the bytes to zero.
//        /// </summary>
//        public int cbClsExtra;

//        /// <summary>
//        /// The number of extra bytes to allocate following the window instance. The system initializes the bytes to zero. If an application uses WNDCLASSEX to register a dialog box created by using the CLASS directive in the resource file, it must set this member to DLGWINDOWEXTRA.
//        /// </summary>
//        public int cbWndExtra;

//        /// <summary>
//        /// A handle to the instance that contains the window procedure for the class.
//        /// </summary>
//        public IntPtr hInstance;

//        /// <summary>
//        /// A handle to the class icon. This member must be a handle to an icon resource. If this member is NULL, the system provides a default icon.
//        /// </summary>
//        public IntPtr hIcon;

//        /// <summary>
//        /// A handle to the class cursor. This member must be a handle to a cursor resource. If this member is NULL, an application must explicitly set the cursor shape whenever the mouse moves into the application's window.
//        /// </summary>
//        public IntPtr hCursor;

//        /// <summary>
//        /// A handle to the class background brush. This member can be a handle to the brush to be used for painting the background, or it can be a color value.
//        /// </summary>
//        public IntPtr hbrBackground;

//        /// <summary>
//        /// Pointer to a null-terminated character string that specifies the resource name of the class menu, as the name appears in the resource file. If you use an integer to identify the menu, use the MAKEINTRESOURCE macro. If this member is NULL, windows belonging to this class have no default menu.
//        /// </summary>
//        [MarshalAs(UnmanagedType.LPWStr)]
//        public string lpszMenuName;

//        /// <summary>
//        /// A pointer to a null-terminated string or is an atom. If this parameter is an atom, it must be a class atom created by a previous call to the RegisterClass or RegisterClassEx function. The atom must be in the low-order word of lpszClassName; the high-order word must be zero.
//        /// </summary>
//        [MarshalAs(UnmanagedType.LPWStr)]
//        public string lpszClassName;

//        /// <summary>
//        /// A handle to a small icon that is associated with the window class. If this member is NULL, the system searches the icon resource specified by the hIcon member for an icon of the appropriate size to use as the small icon.
//        /// </summary>
//        public IntPtr hIconSm;
//    }

//    /// <summary>
//    /// Delegate declaration that matches native WndProc signatures.
//    /// </summary>
//    public delegate IntPtr WndProc(IntPtr hWnd, WM uMsg, IntPtr wParam, IntPtr lParam);

//    /// <summary>
//    /// Delegate declaration that matches native WndProc signatures.
//    /// </summary>
//    public delegate IntPtr WndProcHook(IntPtr hWnd, WM uMsg, IntPtr wParam, IntPtr lParam, ref bool handled);

//    /// <summary>
//    /// Delegate declaration that matches managed WndProc signatures.
//    /// </summary>
//    public delegate IntPtr MessageHandler(WM uMsg, IntPtr wParam, IntPtr lParam, out bool handled);

//    /// <summary>
//    /// The ReleaseDC function releases a device context (DC), freeing it for use by other applications.
//    /// The effect of the ReleaseDC function depends on the type of DC. It frees only common and window DCs. It has no effect on class or private DCs.
//    /// </summary>
//    /// <param name="hWnd">A handle to the window whose DC is to be released.</param>
//    /// <param name="hDC">A handle to the DC to be released.</param>
//    /// <returns>The return value indicates whether the DC was released. If the DC was released, the return value is 1. If the DC was not released, the return value is zero.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
//    public static extern int ReleaseDC([In] IntPtr hWnd, [In] IntPtr hDC);

//    /// <summary>
//    /// Calculates the required size of the window rectangle, based on the desired size of the client rectangle.
//    /// The window rectangle can then be passed to the CreateWindowEx function to create a window whose client area is the desired size.
//    /// </summary>
//    /// <param name="lpRect">A pointer to a RECT structure that contains the coordinates of the top-left and bottom-right corners of the desired client area.</param>
//    /// <param name="dwStyle">The window style of the window whose required size is to be calculated. Note that you cannot specify the WS_OVERLAPPED style.</param>
//    /// <param name="bMenu">Indicates whether the window has a menu.</param>
//    /// <param name="dwExStyle">The extended window style of the window whose required size is to be calculated.</param>
//    /// <returns>If the function succeeds, the return value is nonzero.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Auto, SetLastError = true)]
//    [return: MarshalAs(UnmanagedType.Bool)]
//    public static extern bool AdjustWindowRectEx(
//        [In] ref Rect lpRect,
//        [In] WS dwStyle,
//        [In][MarshalAs(UnmanagedType.Bool)] bool bMenu,
//        [In] WS_EX dwExStyle
//    );

//    /// <summary>
//    /// [Using the ChangeWindowMessageFilter function is not recommended, as it has process-wide scope. Instead, use the ChangeWindowMessageFilterEx function to control access to specific windows as needed. ChangeWindowMessageFilter may not be supported in future versions of Windows.
//    /// <para>Adds or removes a message from the User Interface Privilege Isolation(UIPI) message filter.</para>
//    /// </summary>
//    /// <param name="message">The message to add to or remove from the filter.</param>
//    /// <param name="dwFlag">The action to be performed. One of the following values.</param>
//    /// <returns><see langword="true"/> if successful; otherwise, <see langword="false"/>. To get extended error information, call <see cref="Kernel32.GetLastError"/>.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Auto, SetLastError = true)]
//    [return: MarshalAs(UnmanagedType.Bool)]
//    public static extern bool ChangeWindowMessageFilter([In] WM message, [In] MSGFLT dwFlag);

//    /// <summary>
//    /// Modifies the User Interface Privilege Isolation (UIPI) message filter for a specified window.
//    /// </summary>
//    /// <param name="hWnd">A handle to the window whose UIPI message filter is to be modified.</param>
//    /// <param name="message">The message that the message filter allows through or blocks.</param>
//    /// <param name="action">The action to be performed.</param>
//    /// <param name="pChangeFilterStruct">Optional pointer to a <see cref="CHANGEFILTERSTRUCT"/> structure.</param>
//    /// <returns>If the function succeeds, it returns <see langword="true"/>; otherwise, it returns <see langword="false"/>. To get extended error information, call <see cref="Kernel32.GetLastError"/>.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Auto, SetLastError = true)]
//    [return: MarshalAs(UnmanagedType.Bool)]
//    public static extern bool ChangeWindowMessageFilterEx(
//        [In] IntPtr hWnd,
//        [In] WM message,
//        [In] MSGFLT action,
//        [In, Out, Optional] ref CHANGEFILTERSTRUCT pChangeFilterStruct
//    );

//    /// <summary>
//    /// Places (posts) a message in the message queue associated with the thread that created the specified window and returns without waiting for the thread to process the message.
//    /// <para>Unicode declaration for <see cref="PostMessage"/></para>
//    /// </summary>
//    /// <param name="hWnd">A handle to the window whose window procedure is to receive the message.</param>
//    /// <param name="Msg">The message to be posted.</param>
//    /// <param name="wParam">Additional message-specific information.</param>
//    /// <param name="lParam">Additional message-specific information.~</param>
//    /// <returns>If the function succeeds, the return value is nonzero.</returns>
//    [DllImport(Libraries.User32, SetLastError = true)]
//    [return: MarshalAs(UnmanagedType.Bool)]
//    public static extern bool PostMessageW(
//        [In, Optional] IntPtr hWnd,
//        [In] WM Msg,
//        [In] IntPtr wParam,
//        [In] IntPtr lParam
//    );

//    /// <summary>
//    /// Places (posts) a message in the message queue associated with the thread that created the specified window and returns without waiting for the thread to process the message.
//    /// <para>ANSI declaration for <see cref="PostMessage"/></para>
//    /// </summary>
//    /// <param name="hWnd">A handle to the window whose window procedure is to receive the message.</param>
//    /// <param name="Msg">The message to be posted.</param>
//    /// <param name="wParam">Additional message-specific information.</param>
//    /// <param name="lParam">Additional message-specific information.~</param>
//    /// <returns>If the function succeeds, the return value is nonzero.</returns>
//    [DllImport(Libraries.User32, SetLastError = true)]
//    [return: MarshalAs(UnmanagedType.Bool)]
//    public static extern bool PostMessageA(
//        [In, Optional] IntPtr hWnd,
//        [In] WM Msg,
//        [In] IntPtr wParam,
//        [In] IntPtr lParam
//    );

//    /// <summary>
//    /// Places (posts) a message in the message queue associated with the thread that created the specified window and returns without waiting for the thread to process the message.
//    /// </summary>
//    /// <param name="hWnd">A handle to the window whose window procedure is to receive the message.</param>
//    /// <param name="Msg">The message to be posted.</param>
//    /// <param name="wParam">Additional message-specific information.</param>
//    /// <param name="lParam">Additional message-specific information.~</param>
//    /// <returns>If the function succeeds, the return value is nonzero.</returns>
//    [DllImport(Libraries.User32, SetLastError = true)]
//    [return: MarshalAs(UnmanagedType.Bool)]
//    public static extern bool PostMessage(
//        [In, Optional] IntPtr hWnd,
//        [In] WM Msg,
//        [In] IntPtr wParam,
//        [In] IntPtr lParam
//    );

//    /// <summary>
//    /// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
//    /// </summary>
//    /// <param name="hWnd">A handle to the window whose window procedure will receive the message.</param>
//    /// <param name="wMsg">The message to be sent.</param>
//    /// <param name="wParam">Additional message-specific information.</param>
//    /// <param name="lParam">Additional message-specific information.~</param>
//    /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
//    public static extern int SendMessage(
//        [In] IntPtr hWnd,
//        [In] WM wMsg,
//        [In] IntPtr wParam,
//        [In] IntPtr lParam
//    );

//    /// <summary>
//    /// Creates an overlapped, pop-up, or child window with an extended window style; otherwise,
//    /// this function is identical to the CreateWindow function. For more information about
//    /// creating a window and for full descriptions of the other parameters of CreateWindowEx, see CreateWindow.
//    /// </summary>
//    /// <param name="dwExStyle">The extended window style of the window being created.</param>
//    /// <param name="lpClassName">A null-terminated string or a class atom created by a previous call to the RegisterClass or RegisterClassEx function.</param>
//    /// <param name="lpWindowName">The window name. If the window style specifies a title bar, the window title pointed to by lpWindowName is displayed in the title bar.</param>
//    /// <param name="dwStyle">The style of the window being created. This parameter can be a combination of the window style values, plus the control styles indicated in the Remarks section.</param>
//    /// <param name="x">The initial horizontal position of the window. For an overlapped or pop-up window, the x parameter is the initial x-coordinate of the window's upper-left corner, in screen coordinates.</param>
//    /// <param name="y">The initial vertical position of the window. For an overlapped or pop-up window, the y parameter is the initial y-coordinate of the window's upper-left corner, in screen coordinates.</param>
//    /// <param name="nWidth">The width, in device units, of the window. For overlapped windows, nWidth is the window's width, in screen coordinates, or CW_USEDEFAULT.</param>
//    /// <param name="nHeight">The height, in device units, of the window. For overlapped windows, nHeight is the window's height, in screen coordinates. If the nWidth parameter is set to CW_USEDEFAULT, the system ignores nHeight.</param>
//    /// <param name="hWndParent">A handle to the parent or owner window of the window being created. To create a child window or an owned window, supply a valid window handle. This parameter is optional for pop-up windows.</param>
//    /// <param name="hMenu">A handle to a menu, or specifies a child-window identifier, depending on the window style. For an overlapped or pop-up window, hMenu identifies the menu to be used with the window; it can be NULL if the class menu is to be used.</param>
//    /// <param name="hInstance">A handle to the instance of the module to be associated with the window.</param>
//    /// <param name="lpParam">Pointer to a value to be passed to the window through the CREATESTRUCT structure (lpCreateParams member) pointed to by the lParam param of the WM_CREATE message. This message is sent to the created window by this function before it returns.</param>
//    /// <returns>If the function succeeds, the return value is a handle to the new window.</returns>
//    [DllImport(Libraries.User32, SetLastError = true, CharSet = CharSet.Unicode)]
//    public static extern IntPtr CreateWindowExW(
//        [In] WS_EX dwExStyle,
//        [In, Optional][MarshalAs(UnmanagedType.LPWStr)] string lpClassName,
//        [In, Optional][MarshalAs(UnmanagedType.LPWStr)] string lpWindowName,
//        [In] WS dwStyle,
//        [In] int x,
//        [In] int y,
//        [In] int nWidth,
//        [In] int nHeight,
//        [In, Optional] IntPtr hWndParent,
//        [In, Optional] IntPtr hMenu,
//        [In, Optional] IntPtr hInstance,
//        [In, Optional] IntPtr lpParam
//    );

//    /// <summary>
//    /// Creates an overlapped, pop-up, or child window with an extended window style; otherwise,
//    /// this function is identical to the CreateWindow function. For more information about
//    /// creating a window and for full descriptions of the other parameters of CreateWindowEx, see CreateWindow.
//    /// </summary>
//    /// <param name="dwExStyle">The extended window style of the window being created.</param>
//    /// <param name="lpClassName">A null-terminated string or a class atom created by a previous call to the RegisterClass or RegisterClassEx function.</param>
//    /// <param name="lpWindowName">The window name. If the window style specifies a title bar, the window title pointed to by lpWindowName is displayed in the title bar.</param>
//    /// <param name="dwStyle">The style of the window being created. This parameter can be a combination of the window style values, plus the control styles indicated in the Remarks section.</param>
//    /// <param name="x">The initial horizontal position of the window. For an overlapped or pop-up window, the x parameter is the initial x-coordinate of the window's upper-left corner, in screen coordinates.</param>
//    /// <param name="y">The initial vertical position of the window. For an overlapped or pop-up window, the y parameter is the initial y-coordinate of the window's upper-left corner, in screen coordinates.</param>
//    /// <param name="nWidth">The width, in device units, of the window. For overlapped windows, nWidth is the window's width, in screen coordinates, or CW_USEDEFAULT.</param>
//    /// <param name="nHeight">The height, in device units, of the window. For overlapped windows, nHeight is the window's height, in screen coordinates. If the nWidth parameter is set to CW_USEDEFAULT, the system ignores nHeight.</param>
//    /// <param name="hWndParent">A handle to the parent or owner window of the window being created. To create a child window or an owned window, supply a valid window handle. This parameter is optional for pop-up windows.</param>
//    /// <param name="hMenu">A handle to a menu, or specifies a child-window identifier, depending on the window style. For an overlapped or pop-up window, hMenu identifies the menu to be used with the window; it can be NULL if the class menu is to be used.</param>
//    /// <param name="hInstance">A handle to the instance of the module to be associated with the window.</param>
//    /// <param name="lpParam">Pointer to a value to be passed to the window through the CREATESTRUCT structure (lpCreateParams member) pointed to by the lParam param of the WM_CREATE message. This message is sent to the created window by this function before it returns.</param>
//    /// <returns>If the function succeeds, the return value is a handle to the new window.</returns>
//    public static IntPtr CreateWindowEx(
//        [In] WS_EX dwExStyle,
//        [In] string lpClassName,
//        [In] string lpWindowName,
//        [In] WS dwStyle,
//        [In] int x,
//        [In] int y,
//        [In] int nWidth,
//        [In] int nHeight,
//        [In, Optional] IntPtr hWndParent,
//        [In, Optional] IntPtr hMenu,
//        [In, Optional] IntPtr hInstance,
//        [In, Optional] IntPtr lpParam
//    )
//    {
//        IntPtr ret = CreateWindowExW(
//            dwExStyle,
//            lpClassName,
//            lpWindowName,
//            dwStyle,
//            x,
//            y,
//            nWidth,
//            nHeight,
//            hWndParent,
//            hMenu,
//            hInstance,
//            lpParam
//        );

//        if (ret == IntPtr.Zero)
//        {
//            // HRESULT.ThrowLastError();
//            throw new Exception("Unable to create a window");
//        }

//        return ret;
//    }

//    /// <summary>
//    /// Registers a window class for subsequent use in calls to the CreateWindow or CreateWindowEx function.
//    /// <para>Unicode declaration for <see cref="RegisterClassEx"/></para>
//    /// </summary>
//    /// <param name="lpwcx">A pointer to a <see cref="WNDCLASSEX"/> structure. You must fill the structure with the appropriate class attributes before passing it to the function.</param>
//    /// <returns>If the function succeeds, the return value is a class atom that uniquely identifies the class being registered.</returns>
//    [DllImport(Libraries.User32, SetLastError = true, CharSet = CharSet.Unicode)]
//    public static extern short RegisterClassExW([In] ref WNDCLASSEX lpwcx);

//    /// <summary>
//    /// Registers a window class for subsequent use in calls to the CreateWindow or CreateWindowEx function.
//    /// <para>ANSI declaration for <see cref="RegisterClassEx"/></para>
//    /// </summary>
//    /// <param name="lpwcx">A pointer to a <see cref="WNDCLASSEX"/> structure. You must fill the structure with the appropriate class attributes before passing it to the function.</param>
//    /// <returns>If the function succeeds, the return value is a class atom that uniquely identifies the class being registered.</returns>
//    [DllImport(Libraries.User32, SetLastError = true)]
//    public static extern short RegisterClassExA([In] ref WNDCLASSEX lpwcx);

//    /// <summary>
//    /// Registers a window class for subsequent use in calls to the CreateWindow or CreateWindowEx function.
//    /// </summary>
//    /// <param name="lpwcx">A pointer to a <see cref="WNDCLASSEX"/> structure. You must fill the structure with the appropriate class attributes before passing it to the function.</param>
//    /// <returns>If the function succeeds, the return value is a class atom that uniquely identifies the class being registered.</returns>
//    [DllImport(Libraries.User32, SetLastError = true)]
//    public static extern short RegisterClassEx([In] ref WNDCLASSEX lpwcx);

//    /// <summary>
//    /// Calls the default window procedure to provide default processing for any window messages that an application does not process.
//    /// This function ensures that every message is processed. DefWindowProc is called with the same parameters received by the window procedure.
//    /// <para>Unicode declaration for <see cref="DefWindowProc"/></para>
//    /// </summary>
//    /// <param name="hWnd">A handle to the window procedure that received the message.</param>
//    /// <param name="Msg">The message.</param>
//    /// <param name="wParam">Additional message information. The content of this parameter depends on the value of the Msg parameter.</param>
//    /// <param name="lParam">Additional message information. The content of this parameter depends on the value of the Msg parameter.~</param>
//    /// <returns>The return value is the result of the message processing and depends on the message.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Unicode)]
//    public static extern IntPtr DefWindowProcW(
//        [In] IntPtr hWnd,
//        [In] WM Msg,
//        [In] IntPtr wParam,
//        [In] IntPtr lParam
//    );

//    /// <summary>
//    /// Calls the default window procedure to provide default processing for any window messages that an application does not process.
//    /// This function ensures that every message is processed. DefWindowProc is called with the same parameters received by the window procedure.
//    /// <para>ANSI declaration for <see cref="DefWindowProc"/></para>
//    /// </summary>
//    /// <param name="hWnd">A handle to the window procedure that received the message.</param>
//    /// <param name="Msg">The message.</param>
//    /// <param name="wParam">Additional message information. The content of this parameter depends on the value of the Msg parameter.</param>
//    /// <param name="lParam">Additional message information. The content of this parameter depends on the value of the Msg parameter.~</param>
//    /// <returns>The return value is the result of the message processing and depends on the message.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
//    public static extern IntPtr DefWindowProcA(
//        [In] IntPtr hWnd,
//        [In] WM Msg,
//        [In] IntPtr wParam,
//        [In] IntPtr lParam
//    );

//    /// <summary>
//    /// Calls the default window procedure to provide default processing for any window messages that an application does not process.
//    /// This function ensures that every message is processed. DefWindowProc is called with the same parameters received by the window procedure.
//    /// </summary>
//    /// <param name="hWnd">A handle to the window procedure that received the message.</param>
//    /// <param name="Msg">The message.</param>
//    /// <param name="wParam">Additional message information. The content of this parameter depends on the value of the Msg parameter.</param>
//    /// <param name="lParam">Additional message information. The content of this parameter depends on the value of the Msg parameter.~</param>
//    /// <returns>The return value is the result of the message processing and depends on the message.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
//    public static extern IntPtr DefWindowProc(
//        [In] IntPtr hWnd,
//        [In] WM Msg,
//        [In] IntPtr wParam,
//        [In] IntPtr lParam
//    );

//    /// <summary>
//    /// Retrieves information about the specified window. The function also retrieves the 32-bit (DWORD) value at the specified offset into the extra window memory.
//    /// <para>If you are retrieving a pointer or a handle, this function has been superseded by the <see cref="GetWindowLongPtr"/> function.</para>
//    /// <para>Unicode declaration for <see cref="GetWindowLong(IntPtr, int)"/></para>
//    /// </summary>
//    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
//    /// <param name="nIndex">The zero-based offset to the value to be retrieved.</param>
//    /// <returns>If the function succeeds, the return value is the requested value.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Unicode)]
//    public static extern long GetWindowLongW([In] IntPtr hWnd, [In] int nIndex);

//    /// <summary>
//    /// Retrieves information about the specified window. The function also retrieves the 32-bit (DWORD) value at the specified offset into the extra window memory.
//    /// <para>If you are retrieving a pointer or a handle, this function has been superseded by the <see cref="GetWindowLongPtr"/> function.</para>
//    /// <para>ANSI declaration for <see cref="GetWindowLong(IntPtr, int)"/></para>
//    /// </summary>
//    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
//    /// <param name="nIndex">The zero-based offset to the value to be retrieved.</param>
//    /// <returns>If the function succeeds, the return value is the requested value.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
//    public static extern long GetWindowLongA([In] IntPtr hWnd, [In] int nIndex);

//    /// <summary>
//    /// Retrieves information about the specified window. The function also retrieves the 32-bit (DWORD) value at the specified offset into the extra window memory.
//    /// <para>If you are retrieving a pointer or a handle, this function has been superseded by the <see cref="GetWindowLongPtr"/> function.</para>
//    /// </summary>
//    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
//    /// <param name="nIndex">The zero-based offset to the value to be retrieved.</param>
//    /// <returns>If the function succeeds, the return value is the requested value.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
//    public static extern long GetWindowLong([In] IntPtr hWnd, [In] int nIndex);

//    /// <summary>
//    /// Retrieves information about the specified window. The function also retrieves the 32-bit (DWORD) value at the specified offset into the extra window memory.
//    /// <para>If you are retrieving a pointer or a handle, this function has been superseded by the <see cref="GetWindowLongPtr"/> function.</para>
//    /// </summary>
//    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
//    /// <param name="nIndex">The zero-based offset to the value to be retrieved.</param>
//    /// <returns>If the function succeeds, the return value is the requested value.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
//    public static extern long GetWindowLong([In] IntPtr hWnd, [In] GWL nIndex);

//    /// <summary>
//    /// Retrieves information about the specified window. The function also retrieves the value at a specified offset into the extra window memory.
//    /// <para>Unicode declaration for <see cref="GetWindowLongPtr"/></para>
//    /// </summary>
//    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
//    /// <param name="nIndex">The zero-based offset to the value to be retrieved.</param>
//    /// <returns>If the function succeeds, the return value is the requested value.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
//    public static extern IntPtr GetWindowLongPtrW([In] IntPtr hWnd, [In] int nIndex);

//    /// <summary>
//    /// Retrieves information about the specified window. The function also retrieves the value at a specified offset into the extra window memory.
//    /// <para>ANSI declaration for <see cref="GetWindowLongPtr"/></para>
//    /// </summary>
//    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
//    /// <param name="nIndex">The zero-based offset to the value to be retrieved.</param>
//    /// <returns>If the function succeeds, the return value is the requested value.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
//    public static extern IntPtr GetWindowLongPtrA([In] IntPtr hWnd, [In] int nIndex);

//    /// <summary>
//    /// Retrieves information about the specified window. The function also retrieves the value at a specified offset into the extra window memory.
//    /// </summary>
//    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
//    /// <param name="nIndex">The zero-based offset to the value to be retrieved.</param>
//    /// <returns>If the function succeeds, the return value is the requested value.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
//    public static extern IntPtr GetWindowLongPtr([In] IntPtr hWnd, [In] int nIndex);

//    /// <summary>
//    /// Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
//    /// <para>Note: This function has been superseded by the <see cref="SetWindowLongPtr"/> function. To write code that is compatible with both 32-bit and 64-bit versions of Windows, use the SetWindowLongPtr function.</para>
//    /// <para>Unicode declaration for <see cref="GetWindowLongPtr"/></para>
//    /// </summary>
//    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
//    /// <param name="nIndex">The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer.</param>
//    /// <param name="dwNewLong">The replacement value.</param>
//    /// <returns>If the function succeeds, the return value is the previous value of the specified 32-bit integer.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Unicode)]
//    public static extern long SetWindowLongW([In] IntPtr hWnd, [In] int nIndex, [In] long dwNewLong);

//    /// <summary>
//    /// Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
//    /// <para>Note: This function has been superseded by the <see cref="SetWindowLongPtr"/> function. To write code that is compatible with both 32-bit and 64-bit versions of Windows, use the SetWindowLongPtr function.</para>
//    /// <para>ANSI declaration for <see cref="GetWindowLongPtr"/></para>
//    /// </summary>
//    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
//    /// <param name="nIndex">The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer.</param>
//    /// <param name="dwNewLong">The replacement value.</param>
//    /// <returns>If the function succeeds, the return value is the previous value of the specified 32-bit integer.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
//    public static extern long SetWindowLongA([In] IntPtr hWnd, [In] int nIndex, [In] long dwNewLong);

//    /// <summary>
//    /// Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
//    /// <para>Note: This function has been superseded by the <see cref="SetWindowLongPtr"/> function. To write code that is compatible with both 32-bit and 64-bit versions of Windows, use the SetWindowLongPtr function.</para>
//    /// </summary>
//    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
//    /// <param name="nIndex">The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer.</param>
//    /// <param name="dwNewLong">The replacement value.</param>
//    /// <returns>If the function succeeds, the return value is the previous value of the specified 32-bit integer.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
//    public static extern long SetWindowLong([In] IntPtr hWnd, [In] int nIndex, [In] long dwNewLong);

//    /// <summary>
//    /// Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
//    /// <para>Note: This function has been superseded by the <see cref="SetWindowLongPtr"/> function. To write code that is compatible with both 32-bit and 64-bit versions of Windows, use the SetWindowLongPtr function.</para>
//    /// <para>ANSI declaration for <see cref="GetWindowLongPtr"/></para>
//    /// </summary>
//    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
//    /// <param name="nIndex">The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer.</param>
//    /// <param name="dwNewLong">The replacement value.</param>
//    /// <returns>If the function succeeds, the return value is the previous value of the specified 32-bit integer.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
//    public static extern long SetWindowLong([In] IntPtr hWnd, [In] GWL nIndex, [In] long dwNewLong);

//    /// <summary>
//    /// Changes an attribute of the specified window. The function also sets the 32-bit (long) value at the specified offset into the extra window memory.
//    /// <para>Note: This function has been superseded by the <see cref="SetWindowLongPtr"/> function. To write code that is compatible with both 32-bit and 64-bit versions of Windows, use the SetWindowLongPtr function.</para>
//    /// <para>ANSI declaration for <see cref="GetWindowLongPtr"/></para>
//    /// </summary>
//    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
//    /// <param name="nIndex">The zero-based offset to the value to be set. Valid values are in the range zero through the number of bytes of extra window memory, minus the size of an integer.</param>
//    /// <param name="dwNewLong">New window style.</param>
//    /// <returns>If the function succeeds, the return value is the previous value of the specified 32-bit integer.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
//    public static extern long SetWindowLong([In] IntPtr hWnd, [In] GWL nIndex, [In] WS dwNewLong);

//    /// <summary>
//    /// Changes an attribute of the specified window. The function also sets a value at the specified offset in the extra window memory.
//    /// <para>Unicode declaration for <see cref="SetWindowLongPtr"/></para>
//    /// </summary>
//    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
//    /// <param name="nIndex">The zero-based offset to the value to be set.</param>
//    /// <param name="dwNewLong">The replacement value.</param>
//    /// <returns>If the function succeeds, the return value is the previous value of the specified offset.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
//    public static extern IntPtr SetWindowLongPtrW([In] IntPtr hWnd, [In] int nIndex, [In] IntPtr dwNewLong);

//    /// <summary>
//    /// Changes an attribute of the specified window. The function also sets a value at the specified offset in the extra window memory.
//    /// <para>ANSI declaration for <see cref="SetWindowLongPtr"/></para>
//    /// </summary>
//    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
//    /// <param name="nIndex">The zero-based offset to the value to be set.</param>
//    /// <param name="dwNewLong">The replacement value.</param>
//    /// <returns>If the function succeeds, the return value is the previous value of the specified offset.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
//    public static extern IntPtr SetWindowLongPtrA([In] IntPtr hWnd, [In] int nIndex, [In] IntPtr dwNewLong);

//    /// <summary>
//    /// Changes an attribute of the specified window. The function also sets a value at the specified offset in the extra window memory.
//    /// </summary>
//    /// <param name="hWnd">A handle to the window and, indirectly, the class to which the window belongs.</param>
//    /// <param name="nIndex">The zero-based offset to the value to be set.</param>
//    /// <param name="dwNewLong">The replacement value.</param>
//    /// <returns>If the function succeeds, the return value is the previous value of the specified offset.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
//    public static extern IntPtr SetWindowLongPtr([In] IntPtr hWnd, [In] int nIndex, [In] IntPtr dwNewLong);

//    /// <summary>
//    /// Changes an attribute of the specified window.
//    /// </summary>
//    [DllImport(Libraries.User32, CharSet = CharSet.Auto, SetLastError = true)]
//    public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

//    /// <summary>
//    /// Destroys an icon and frees any memory the icon occupied.
//    /// </summary>
//    /// <param name="handle">A handle to the icon to be destroyed. The icon must not be in use.</param>
//    /// <returns>If the function succeeds, the return value is nonzero.</returns>
//    [DllImport(Libraries.User32)]
//    [return: MarshalAs(UnmanagedType.Bool)]
//    public static extern bool DestroyIcon([In] IntPtr handle);

//    /// <summary>
//    /// Determines whether the specified window handle identifies an existing window.
//    /// </summary>
//    /// <param name="hWnd">A handle to the window to be tested.</param>
//    /// <returns>If the window handle identifies an existing window, the return value is nonzero.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
//    [return: MarshalAs(UnmanagedType.Bool)]
//    public static extern bool IsWindow([In] IntPtr hWnd);

//    /// <summary>
//    /// Destroys the specified window. The function sends WM_DESTROY and WM_NCDESTROY messages to the window to deactivate it and remove the keyboard focus from it.
//    /// </summary>
//    /// <param name="hWnd">A handle to the window to be destroyed.</param>
//    /// <returns>If the function succeeds, the return value is nonzero.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Auto, SetLastError = true)]
//    [return: MarshalAs(UnmanagedType.Bool)]
//    public static extern bool DestroyWindow([In] IntPtr hWnd);

//    /// <summary>
//    /// Retrieves the show state and the restored, minimized, and maximized positions of the specified window.
//    /// </summary>
//    /// <param name="hWnd">A handle to the window.</param>
//    /// <param name="lpwndpl">A pointer to the <see cref="WINDOWPLACEMENT"/> structure that receives the show state and position information. Before calling GetWindowPlacement, set the length member to sizeof(WINDOWPLACEMENT). GetWindowPlacement fails if lpwndpl-> length is not set correctly.</param>
//    /// <returns>If the function succeeds, the return value is nonzero.</returns>
//    [DllImport(Libraries.User32, SetLastError = true)]
//    [return: MarshalAs(UnmanagedType.Bool)]
//    public static extern bool GetWindowPlacement([In] IntPtr hWnd, [In] WINDOWPLACEMENT lpwndpl);

//    /// <summary>
//    /// Retrieves the dimensions of the bounding rectangle of the specified window. The dimensions are given in screen coordinates that are relative to the upper-left corner of the screen.
//    /// </summary>
//    /// <param name="hWnd">A handle to the window.</param>
//    /// <param name="lpRect">A pointer to a RECT structure that receives the screen coordinates of the upper-left and lower-right corners of the window.</param>
//    /// <returns>If the function succeeds, the return value is nonzero.</returns>
//    [DllImport(Libraries.User32, SetLastError = true)]
//    [return: MarshalAs(UnmanagedType.Bool)]
//    public static extern bool GetWindowRect([In] IntPtr hWnd, [Out] out Rect lpRect);

//    /// <summary>
//    /// Determines the visibility state of the specified window.
//    /// </summary>
//    /// <param name="hWnd">A handle to the window to be tested.</param>
//    /// <returns>If the specified window, its parent window, its parent's parent window, and so forth, have the WS_VISIBLE style, the return value is nonzero. Otherwise, the return value is zero.</returns>
//    [DllImport(Libraries.User32)]
//    [return: MarshalAs(UnmanagedType.Bool)]
//    public static extern bool IsWindowVisible([In] IntPtr hWnd);

//    /// <summary>
//    /// Determines whether the specified window is enabled for mouse and keyboard input.
//    /// </summary>
//    /// <param name="hWnd">A handle to the window to be tested.</param>
//    /// <returns>If the window is enabled, the return value is nonzero.</returns>
//    [DllImport(Libraries.User32, ExactSpelling = true)]
//    internal static extern bool IsWindowEnabled(IntPtr hWnd);

//    /// <summary>
//    /// The MonitorFromWindow function retrieves a handle to the display monitor that has the largest area of intersection with the bounding rectangle of a specified window.
//    /// </summary>
//    /// <param name="hWnd">A handle to the window of interest.</param>
//    /// <param name="dwFlags">Determines the function's return value if the window does not intersect any display monitor.</param>
//    /// <returns>If the window intersects one or more display monitor rectangles, the return value is an HMONITOR handle to the display monitor that has the largest area of intersection with the window.</returns>
//    [DllImport(Libraries.User32)]
//    public static extern IntPtr MonitorFromWindow(IntPtr hWnd, uint dwFlags);

//    /// <summary>
//    /// Retrieves the specified system metric or system configuration setting.
//    /// Note that all dimensions retrieved by GetSystemMetrics are in pixels.
//    /// </summary>
//    /// <param name="nIndex">The system metric or configuration setting to be retrieved. This parameter can be one of the <see cref="SM"/> values.
//    /// Note that all SM_CX* values are widths and all SM_CY* values are heights. Also note that all settings designed to return Boolean data represent <see langword="true"/> as any nonzero value, and <see langword="false"/> as a zero value.</param>
//    /// <returns>If the function succeeds, the return value is the requested system metric or configuration setting.</returns>
//    [DllImport(Libraries.User32)]
//    public static extern int GetSystemMetrics([In] SM nIndex);

//    /// <summary>
//    /// Defines a new window message that is guaranteed to be unique throughout the system. The message value can be used when sending or posting messages.
//    /// <para>Unicode declaration for <see cref="RegisterWindowMessage"/></para>
//    /// </summary>
//    /// <param name="lpString">The message to be registered.</param>
//    /// <returns>If the message is successfully registered, the return value is a message identifier in the range 0xC000 through 0xFFFF.</returns>
//    [DllImport(Libraries.User32, SetLastError = true, CharSet = CharSet.Unicode)]
//    public static extern uint RegisterWindowMessageW([MarshalAs(UnmanagedType.LPWStr)] string lpString);

//    /// <summary>
//    /// Defines a new window message that is guaranteed to be unique throughout the system. The message value can be used when sending or posting messages.
//    /// <para>ANSI declaration for <see cref="RegisterWindowMessage"/></para>
//    /// </summary>
//    /// <param name="lpString">The message to be registered.</param>
//    /// <returns>If the message is successfully registered, the return value is a message identifier in the range 0xC000 through 0xFFFF.</returns>
//    [DllImport(Libraries.User32, SetLastError = true, CharSet = CharSet.Auto)]
//    public static extern uint RegisterWindowMessageA([MarshalAs(UnmanagedType.LPWStr)] string lpString);

//    /// <summary>
//    /// Defines a new window message that is guaranteed to be unique throughout the system. The message value can be used when sending or posting messages.
//    /// </summary>
//    /// <param name="lpString">The message to be registered.</param>
//    /// <returns>If the message is successfully registered, the return value is a message identifier in the range 0xC000 through 0xFFFF.</returns>
//    [DllImport(Libraries.User32, SetLastError = true, CharSet = CharSet.Auto)]
//    public static extern uint RegisterWindowMessage([MarshalAs(UnmanagedType.LPWStr)] string lpString);

//    /// <summary>
//    /// Activates a window. The window must be attached to the calling thread's message queue.
//    /// </summary>
//    /// <param name="hWnd">A handle to the top-level window to be activated.</param>
//    /// <returns>If the function succeeds, the return value is the handle to the window that was previously active.</returns>
//    [DllImport(Libraries.User32, SetLastError = true)]
//    public static extern IntPtr SetActiveWindow(IntPtr hWnd);

//    /// <summary>
//    /// Brings the thread that created the specified window into the foreground and activates the window.
//    /// Keyboard input is directed to the window, and various visual cues are changed for the user.
//    /// The system assigns a slightly higher priority to the thread that created the foreground window than it does to other threads.
//    /// </summary>
//    /// <param name="hWnd">A handle to the window that should be activated and brought to the foreground.</param>
//    /// <returns>If the window was brought to the foreground, the return value is nonzero.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
//    [return: MarshalAs(UnmanagedType.Bool)]
//    public static extern bool SetForegroundWindow(IntPtr hWnd);

//    /// <summary>
//    /// Retrieves the position of the mouse cursor, in screen coordinates.
//    /// </summary>
//    /// <param name="lpPoint">A pointer to a <see cref="WinDef.POINT"/> structure that receives the screen coordinates of the cursor.</param>
//    /// <returns>Returns nonzero if successful or zero otherwise. To get extended error information, call <see cref="Kernel32.GetLastError"/>.</returns>
//    [DllImport(Libraries.User32, SetLastError = true)]
//    [return: MarshalAs(UnmanagedType.Bool)]
//    public static extern bool GetCursorPos([Out] out POINT lpPoint);

//    [DllImport(Libraries.User32)]
//    public static extern bool UnionRect(out RECT rcDst, ref RECT rc1, ref RECT rc2);

//    [DllImport(Libraries.User32, SetLastError = true)]
//    public static extern bool IntersectRect(ref RECT rcDest, ref RECT rc1, ref RECT rc2);

//    [DllImport(Libraries.User32)]
//    public static extern IntPtr GetShellWindow();

//    [DllImport(Libraries.User32, CharSet = CharSet.Unicode)]
//    public static extern int MapVirtualKey(int nVirtKey, int nMapType);

//    [DllImport(Libraries.User32)]
//    public static extern int GetSysColor(int nIndex);

//    [DllImport(Libraries.User32)]
//    public static extern IntPtr GetSystemMenu(
//        [In] IntPtr hWnd,
//        [In][MarshalAs(UnmanagedType.Bool)] bool bRevert
//    );

//    [DllImport(Libraries.User32, EntryPoint = "EnableMenuItem")]
//    private static extern int _EnableMenuItem([In] IntPtr hMenu, [In] SC uIDEnableItem, [In] MF uEnable);

//    /// <summary>
//    /// Enables, disables, or grays the specified menu item.
//    /// </summary>
//    /// <param name="hMenu">A handle to the menu.</param>
//    /// <param name="uIDEnableItem">The menu item to be enabled, disabled, or grayed, as determined by the uEnable parameter.</param>
//    /// <param name="uEnable">Controls the interpretation of the uIDEnableItem parameter and indicate whether the menu item is enabled, disabled, or grayed.</param>
//    /// <returns>The return value specifies the previous state of the menu item (it is either MF_DISABLED, MF_ENABLED, or MF_GRAYED). If the menu item does not exist, the return value is -1 (<see cref="MF.DOES_NOT_EXIST"/>).</returns>
//    public static MF EnableMenuItem([In] IntPtr hMenu, [In] SC uIDEnableItem, [In] MF uEnable)
//    {
//        // Returns the previous state of the menu item, or -1 if the menu item does not exist.
//        int iRet = _EnableMenuItem(hMenu, uIDEnableItem, uEnable);
//        return (MF)iRet;
//    }

//    [DllImport(Libraries.User32, EntryPoint = "SetWindowRgn", SetLastError = true)]
//    private static extern int _SetWindowRgn(
//        [In] IntPtr hWnd,
//        [In] IntPtr hRgn,
//        [In][MarshalAs(UnmanagedType.Bool)] bool bRedraw
//    );

//    /// <summary>
//    /// The SetWindowRgn function sets the window region of a window. The window region determines the area within the window where the system permits drawing. The system does not display any portion of a window that lies outside of the window region.
//    /// </summary>
//    /// <param name="hWnd">A handle to the window whose window region is to be set.</param>
//    /// <param name="hRgn">A handle to a region. The function sets the window region of the window to this region.</param>
//    /// <param name="bRedraw">Specifies whether the system redraws the window after setting the window region. If bRedraw is <see langword="true"/>, the system does so; otherwise, it does not.</param>
//    /// <exception cref="Win32Exception">Native method returned HRESULT.</exception>
//    public static void SetWindowRgn([In] IntPtr hWnd, [In] IntPtr hRgn, [In] bool bRedraw)
//    {
//        var err = _SetWindowRgn(hWnd, hRgn, bRedraw);

//        if (err == 0)
//        {
//            throw new Win32Exception();
//        }
//    }

//    [DllImport(Libraries.User32, EntryPoint = "SetWindowPos", SetLastError = true)]
//    [return: MarshalAs(UnmanagedType.Bool)]
//    private static extern bool _SetWindowPos(
//        [In] IntPtr hWnd,
//        [In, Optional] IntPtr hWndInsertAfter,
//        [In] int x,
//        [In] int y,
//        [In] int cx,
//        [In] int cy,
//        [In] SWP uFlags
//    );

//    /// <summary>
//    /// Changes the size, position, and Z order of a child, pop-up, or top-level window. These windows are ordered according to their appearance on the screen. The topmost window receives the highest rank and is the first window in the Z order.
//    /// </summary>
//    /// <param name="hWnd">A handle to the window.</param>
//    /// <param name="hWndInsertAfter">A handle to the window to precede the positioned window in the Z order.</param>
//    /// <param name="x">The new position of the left side of the window, in client coordinates.</param>
//    /// <param name="y">The new position of the top of the window, in client coordinates.</param>
//    /// <param name="cx">The new width of the window, in pixels.</param>
//    /// <param name="cy">The new height of the window, in pixels.</param>
//    /// <param name="uFlags">The window sizing and positioning flags.</param>
//    /// <returns>If the function succeeds, the return value is nonzero.</returns>
//    public static bool SetWindowPos(
//        [In] IntPtr hWnd,
//        [In, Optional] IntPtr hWndInsertAfter,
//        [In] int x,
//        [In] int y,
//        [In] int cx,
//        [In] int cy,
//        [In] SWP uFlags
//    )
//    {
//        if (!_SetWindowPos(hWnd, hWndInsertAfter, x, y, cx, cy, uFlags))
//        {
//            // If this fails it's never worth taking down the process.  Let the caller deal with the error if they want.
//            return false;
//        }

//        return true;
//    }

//    /// <summary>
//    /// Sets the process-default DPI awareness to system-DPI awareness. This is equivalent to calling SetProcessDpiAwarenessContext with a DPI_AWARENESS_CONTEXT value of DPI_AWARENESS_CONTEXT_SYSTEM_AWARE.
//    /// </summary>
//    [DllImport(Libraries.User32)]
//    public static extern void SetProcessDPIAware();

//    /// <summary>
//    /// Sets various information regarding DWM window attributes.
//    /// </summary>
//    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
//    public static extern int SetWindowCompositionAttribute(
//        [In] IntPtr hWnd,
//        [In, Out] ref WINCOMPATTRDATA data
//    );

//    /// <summary>
//    /// Sets various information regarding DWM window attributes.
//    /// </summary>
//    [DllImport(Libraries.User32, CharSet = CharSet.Auto)]
//    public static extern int GetWindowCompositionAttribute(
//        [In] IntPtr hWnd,
//        [In, Out] ref WINCOMPATTRDATA data
//    );

//    /// <summary>
//    /// Returns the dots per inch (dpi) value for the specified window.
//    /// </summary>
//    /// <param name="hWnd">The window that you want to get information about.</param>
//    /// <returns>The DPI for the window, which depends on the DPI_AWARENESS of the window.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
//    public static extern uint GetDpiForWindow([In] IntPtr hWnd);

//    /// <summary>
//    /// Returns the dots per inch (dpi) value for the specified window.
//    /// </summary>
//    /// <param name="hwnd">The window that you want to get information about.</param>
//    /// <returns>The DPI for the window, which depends on the DPI_AWARENESS of the window.</returns>
//    [DllImport(Libraries.User32, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Winapi)]
//    public static extern uint GetDpiForWindow([In] HandleRef hwnd);
//}

//#pragma warning restore SA1300 // Element should begin with upper-case letter
//#pragma warning restore SA1307 // Accessible fields should begin with upper-case letter
//#pragma warning restore SA1401 // Fields should be private

//internal static class Libraries
//{
//    /*internal const string Advapi32 = "advapi32.dll";
//    internal const string BCrypt = "BCrypt.dll";
//    internal const string CoreComm_L1_1_1 = "api-ms-win-core-comm-l1-1-1.dll";
//    internal const string Crypt32 = "crypt32.dll";*/
//    internal const string Dwmapi = "dwmapi.dll";

//    /*internal const string Error_L1 = "api-ms-win-core-winrt-error-l1-1-0.dll";
//    internal const string HttpApi = "httpapi.dll";
//    internal const string IpHlpApi = "iphlpapi.dll";*/
//    internal const string Kernel32 = "kernel32.dll";

//    /*internal const string Memory_L1_3 = "api-ms-win-core-memory-l1-1-3.dll";
//    internal const string Mswsock = "mswsock.dll";
//    internal const string NCrypt = "ncrypt.dll";
//    internal const string NtDll = "ntdll.dll";
//    internal const string Odbc32 = "odbc32.dll";
//    internal const string OleAut32 = "oleaut32.dll";
//    internal const string PerfCounter = "perfcounter.dll";
//    internal const string RoBuffer = "api-ms-win-core-winrt-robuffer-l1-1-0.dll";
//    internal const string Secur32 = "secur32.dll";*/
//    internal const string Shell32 = "shell32.dll";

//    /*internal const string SspiCli = "sspicli.dll";*/
//    internal const string User32 = "user32.dll";
//    internal const string UxTheme = "uxtheme.dll";

//    /*internal const string Gdi32 = "gdi32.dll";
//    internal const string Gdip = "gdiplus.dll";*/
//    internal const string Version = "version.dll";
//    /*internal const string WebSocket = "websocket.dll";
//    internal const string WinHttp = "winhttp.dll";
//    internal const string WinMM = "winmm.dll";
//    internal const string Ws2_32 = "ws2_32.dll";
//    internal const string Wtsapi32 = "wtsapi32.dll";
//    internal const string CompressionNative = "System.IO.Compression.Native.dll";*/
//}

//[StructLayout(LayoutKind.Sequential)]
//public struct RECTL
//{
//    private long _left;
//    private long _top;
//    private long _right;
//    private long _bottom;

//    /// <summary>
//    /// Gets or sets the x-coordinate of the upper-left corner of the rectangle.
//    /// </summary>
//    public long Left
//    {
//        readonly get { return _left; }
//        set { _left = value; }
//    }

//    /// <summary>
//    /// Gets or sets the x-coordinate of the lower-right corner of the rectangle.
//    /// </summary>
//    public long Right
//    {
//        readonly get { return _right; }
//        set { _right = value; }
//    }

//    /// <summary>
//    /// Gets or sets the y-coordinate of the upper-left corner of the rectangle.
//    /// </summary>
//    public long Top
//    {
//        readonly get { return _top; }
//        set { _top = value; }
//    }

//    /// <summary>
//    /// Gets or sets the y-coordinate of the lower-right corner of the rectangle.
//    /// </summary>
//    public long Bottom
//    {
//        readonly get { return _bottom; }
//        set { _bottom = value; }
//    }

//    /// <summary>
//    /// Gets the width of the rectangle.
//    /// </summary>
//    public readonly long Width
//    {
//        get { return _right - _left; }
//    }

//    /// <summary>
//    /// Gets the height of the rectangle.
//    /// </summary>
//    public readonly long Height
//    {
//        get { return _bottom - _top; }
//    }

//    /// <summary>
//    /// Gets the position of the rectangle.
//    /// </summary>
//    public POINTL Position
//    {
//        get { return new POINTL { x = _left, y = _top }; }
//    }

//    /// <summary>
//    /// Gets the size of the rectangle.
//    /// </summary>
//    public SIZE Size
//    {
//        get { return new SIZE { cx = Width, cy = Height }; }
//    }

//    /// <summary>
//    /// Sets offset of the rectangle.
//    /// </summary>
//    public void Offset(int dx, int dy)
//    {
//        _left += dx;
//        _top += dy;
//        _right += dx;
//        _bottom += dy;
//    }

//    /// <summary>
//    /// Combines two RECTLs
//    /// </summary>
//    public static RECTL Union(RECTL rect1, RECTL rect2)
//    {
//        return new RECTL
//        {
//            Left = Math.Min(rect1.Left, rect2.Left),
//            Top = Math.Min(rect1.Top, rect2.Top),
//            Right = Math.Max(rect1.Right, rect2.Right),
//            Bottom = Math.Max(rect1.Bottom, rect2.Bottom),
//        };
//    }

//    /// <inheritdoc />
//    public override readonly bool Equals(object? obj)
//    {
//        if (obj is not RECTL)
//        {
//            return false;
//        }

//        try
//        {
//            var rc = (RECTL)obj;
//            return rc._bottom == _bottom && rc._left == _left && rc._right == _right && rc._top == _top;
//        }
//        catch (InvalidCastException)
//        {
//            return false;
//        }
//    }

//    /// <inheritdoc />
//    public override readonly int GetHashCode()
//    {
//        return _top.GetHashCode() ^ _bottom.GetHashCode() ^ _left.GetHashCode() ^ _right.GetHashCode();
//    }

//    public static bool operator ==(RECTL left, RECTL right)
//    {
//        return left.Equals(right);
//    }

//    public static bool operator !=(RECTL left, RECTL right)
//    {
//        return !(left == right);
//    }
//}

//// This Source Code Form is subject to the terms of the MIT License.
//// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
//// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
//// All Rights Reserved.

///* This Source Code is partially based on reverse engineering of the Windows Operating System,
//   and is intended for use on Windows systems only.
//   This Source Code is partially based on the source code provided by the .NET Foundation. */

//// ReSharper disable InconsistentNaming
//#pragma warning disable SA1307 // Accessible fields should begin with upper-case letter

///// <summary>
///// The POINT structure defines the x- and y-coordinates of a point.
///// </summary>
//[StructLayout(LayoutKind.Sequential)]
//public struct POINT
//{
//    /// <summary>
//    /// Specifies the x-coordinate of the point.
//    /// </summary>
//    public int x;

//    /// <summary>
//    /// Specifies the y-coordinate of the point.
//    /// </summary>
//    public int y;
//}

//#pragma warning restore SA1307 // Accessible fields should begin with upper-case letter

//// This Source Code Form is subject to the terms of the MIT License.
//// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
//// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
//// All Rights Reserved.

///* This Source Code is partially based on reverse engineering of the Windows Operating System,
//   and is intended for use on Windows systems only.
//   This Source Code is partially based on the source code provided by the .NET Foundation. */

//// ReSharper disable InconsistentNaming
//#pragma warning disable SA1307 // Accessible fields should begin with upper-case letter

///// <summary>
///// The <see cref="POINTL"/> structure defines the x- and y-coordinates of a point.
///// </summary>
//[StructLayout(LayoutKind.Sequential)]
//public struct POINTL
//{
//    /// <summary>
//    /// Specifies the x-coordinate of the point.
//    /// </summary>
//    public long x;

//    /// <summary>
//    /// Specifies the y-coordinate of the point.
//    /// </summary>
//    public long y;
//}

//#pragma warning restore SA1307 // Accessible fields should begin with upper-case letter

//// This Source Code Form is subject to the terms of the MIT License.
//// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
//// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
//// All Rights Reserved.

///* This Source Code is partially based on reverse engineering of the Windows Operating System,
//   and is intended for use on Windows systems only.
//   This Source Code is partially based on the source code provided by the .NET Foundation. */

//// ReSharper disable InconsistentNaming
//#pragma warning disable SA1307 // Accessible fields should begin with upper-case letter

///// <summary>
///// The SIZE structure defines the width and height of a rectangle.
///// </summary>
//[StructLayout(LayoutKind.Sequential)]
//public struct SIZE
//{
//    /// <summary>
//    /// Specifies the rectangle's width. The units depend on which function uses this structure.
//    /// </summary>
//    public long cx;

//    /// <summary>
//    /// Specifies the rectangle's height. The units depend on which function uses this structure.
//    /// </summary>
//    public long cy;
//}

//#pragma warning restore SA1307 // Accessible fields should begin with upper-case letter

//// This Source Code Form is subject to the terms of the MIT License.
//// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
//// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
//// All Rights Reserved.

///* This Source Code is partially based on reverse engineering of the Windows Operating System,
//   and is intended for use on Windows systems only.
//   This Source Code is partially based on the source code provided by the .NET Foundation. */


//// ReSharper disable InconsistentNaming

///// <summary>
///// The RECT structure defines a rectangle by the coordinates of its upper-left and lower-right corners.
///// </summary>
//[StructLayout(LayoutKind.Sequential)]
//public struct RECT
//{
//    private int _left;
//    private int _top;
//    private int _right;
//    private int _bottom;

//    /// <summary>
//    /// Gets or sets the x-coordinate of the upper-left corner of the rectangle.
//    /// </summary>
//    public int Left
//    {
//        readonly get { return _left; }
//        set { _left = value; }
//    }

//    /// <summary>
//    /// Gets or sets the x-coordinate of the lower-right corner of the rectangle.
//    /// </summary>
//    public int Right
//    {
//        readonly get { return _right; }
//        set { _right = value; }
//    }

//    /// <summary>
//    /// Gets or sets the y-coordinate of the upper-left corner of the rectangle.
//    /// </summary>
//    public int Top
//    {
//        readonly get { return _top; }
//        set { _top = value; }
//    }

//    /// <summary>
//    /// Gets or sets the y-coordinate of the lower-right corner of the rectangle.
//    /// </summary>
//    public int Bottom
//    {
//        readonly get { return _bottom; }
//        set { _bottom = value; }
//    }

//    /// <summary>
//    /// Gets the width of the rectangle.
//    /// </summary>
//    public readonly int Width
//    {
//        get { return _right - _left; }
//    }

//    /// <summary>
//    /// Gets the height of the rectangle.
//    /// </summary>
//    public readonly int Height
//    {
//        get { return _bottom - _top; }
//    }

//    /// <summary>
//    /// Gets the position of the rectangle.
//    /// </summary>
//    public POINT Position
//    {
//        get { return new POINT { x = _left, y = _top }; }
//    }

//    /// <summary>
//    /// Gets the size of the rectangle.
//    /// </summary>
//    public SIZE Size
//    {
//        get { return new SIZE { cx = Width, cy = Height }; }
//    }

//    /// <summary>
//    /// Sets offset of the rectangle.
//    /// </summary>
//    public void Offset(int dx, int dy)
//    {
//        _left += dx;
//        _top += dy;
//        _right += dx;
//        _bottom += dy;
//    }

//    /// <summary>
//    /// Combines two RECTs.
//    /// </summary>
//    public static RECT Union(RECT rect1, RECT rect2)
//    {
//        return new RECT
//        {
//            Left = Math.Min(rect1.Left, rect2.Left),
//            Top = Math.Min(rect1.Top, rect2.Top),
//            Right = Math.Max(rect1.Right, rect2.Right),
//            Bottom = Math.Max(rect1.Bottom, rect2.Bottom),
//        };
//    }

//    /// <inheritdoc />
//    public override readonly bool Equals(object? obj)
//    {
//        if (obj is not RECT)
//        {
//            return false;
//        }

//        try
//        {
//            var rc = (RECT)obj;

//            return rc._bottom == _bottom && rc._left == _left && rc._right == _right && rc._top == _top;
//        }
//        catch (InvalidCastException)
//        {
//            return false;
//        }
//    }

//    /// <inheritdoc />
//    public override readonly int GetHashCode()
//    {
//        return _top.GetHashCode() ^ _bottom.GetHashCode() ^ _left.GetHashCode() ^ _right.GetHashCode();
//    }

//    public static bool operator ==(RECT left, RECT right)
//    {
//        return left.Equals(right);
//    }

//    public static bool operator !=(RECT left, RECT right)
//    {
//        return !(left == right);
//    }
//}



//internal static class DpiHelper
//{
//    [ThreadStatic]
//    private static Matrix _transformToDevice;

//    [ThreadStatic]
//    private static Matrix _transformToDip;

//    /// <summary>
//    /// Default DPI value.
//    /// </summary>
//    internal const int DefaultDpi = 96;

//    /*
//    /// <summary>
//    /// Occurs when application DPI is changed.
//    /// </summary>
//    public static event EventHandler<DpiChangedEventArgs> DpiChanged;
//    */

//    /// <summary>
//    /// Gets DPI of the selected <see cref="Window"/>.
//    /// </summary>
//    /// <param name="window">The window that you want to get information about.</param>
//    public static DisplayDpi GetWindowDpi(System.Windows.Window? window)
//    {
//        if (window is null)
//        {
//            return new DisplayDpi(DefaultDpi, DefaultDpi);
//        }

//        return GetWindowDpi(new WindowInteropHelper(window).Handle);
//    }

//    /// <summary>
//    /// Gets DPI of the selected <see cref="Window"/> based on it's handle.
//    /// </summary>
//    /// <param name="windowHandle">Handle of the window that you want to get information about.</param>
//    public static DisplayDpi GetWindowDpi(IntPtr windowHandle)
//    {
//        if (windowHandle == IntPtr.Zero || !UnsafeNativeMethods.IsValidWindow(windowHandle))
//        {
//            return new DisplayDpi(DefaultDpi, DefaultDpi);
//        }

//        var windowDpi = (int)User32.GetDpiForWindow(windowHandle);

//        return new DisplayDpi(windowDpi, windowDpi);
//    }

//    // TODO: Look into utilizing preprocessor symbols for more functionality
//    // ----
//    // There is an opportunity to check against NET46 if we can use
//    // VisualTreeHelper in this class. We are currently not utilizing
//    // it because it is not available in .NET Framework 4.6 (available
//    // starting 4.6.2). For now, there is no need to overcomplicate this
//    // solution for some infrequent DPI calculations. However, if this
//    // becomes more central to various implementations, we may want to
//    // look into fleshing it out a bit further.
//    // ----
//    // Reference: https://docs.microsoft.com/en-us/dotnet/standard/frameworks

//    /// <summary>
//    /// Gets the DPI values from <see cref="SystemParameters"/>.
//    /// </summary>
//    /// <returns>The DPI values from <see cref="SystemParameters"/>. If the property cannot be accessed, the default value <see langword="96"/> is returned.</returns>
//    public static DisplayDpi GetSystemDpi()
//    {
//        System.Reflection.PropertyInfo? dpiXProperty = typeof(SystemParameters).GetProperty(
//            "DpiX",
//            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static
//        );

//        if (dpiXProperty == null)
//        {
//            return new DisplayDpi(DefaultDpi, DefaultDpi);
//        }

//        System.Reflection.PropertyInfo? dpiYProperty = typeof(SystemParameters).GetProperty(
//            "Dpi",
//            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static
//        );

//        if (dpiYProperty == null)
//        {
//            return new DisplayDpi(DefaultDpi, DefaultDpi);
//        }

//        return new DisplayDpi(
//            (int)dpiXProperty.GetValue(null, null)!,
//            (int)dpiYProperty.GetValue(null, null)!
//        );
//    }

//    /// <summary>
//    /// Convert a point in device independent pixels (1/96") to a point in the system coordinates.
//    /// </summary>
//    /// <param name="logicalPoint">A point in the logical coordinate system.</param>
//    /// <param name="dpiScaleX">Horizontal DPI scale.</param>
//    /// <param name="dpiScaleY">Vertical DPI scale.</param>
//    /// <returns>Returns the parameter converted to the system's coordinates.</returns>
//    public static System.Windows.Point LogicalPixelsToDevice(System.Windows.Point logicalPoint, double dpiScaleX, double dpiScaleY)
//    {
//        _transformToDevice = Matrix.Identity;
//        _transformToDevice.Scale(dpiScaleX, dpiScaleY);

//        return _transformToDevice.Transform(logicalPoint);
//    }

//    /// <summary>
//    /// Convert a point in system coordinates to a point in device independent pixels (1/96").
//    /// </summary>
//    /// <returns>Returns the parameter converted to the device independent coordinate system.</returns>
//    public static System.Windows.Point DevicePixelsToLogical(System.Windows.Point devicePoint, double dpiScaleX, double dpiScaleY)
//    {
//        _transformToDip = Matrix.Identity;
//        _transformToDip.Scale(1d / dpiScaleX, 1d / dpiScaleY);

//        return _transformToDip.Transform(devicePoint);
//    }

//    public static Rect LogicalRectToDevice(Rect logicalRectangle, double dpiScaleX, double dpiScaleY)
//    {
//        System.Windows.Point topLeft = LogicalPixelsToDevice(
//            new System.Windows.Point(logicalRectangle.Left, logicalRectangle.Top),
//            dpiScaleX,
//            dpiScaleY
//        );
//        System.Windows.Point bottomRight = LogicalPixelsToDevice(
//            new System.Windows.Point(logicalRectangle.Right, logicalRectangle.Bottom),
//            dpiScaleX,
//            dpiScaleY
//        );

//        return new Rect(topLeft, bottomRight);
//    }

//    public static Rect DeviceRectToLogical(Rect deviceRectangle, double dpiScaleX, double dpiScaleY)
//    {
//        System.Windows.Point topLeft = DevicePixelsToLogical(
//            new System.Windows.Point(deviceRectangle.Left, deviceRectangle.Top),
//            dpiScaleX,
//            dpiScaleY
//        );
//        System.Windows.Point bottomRight = DevicePixelsToLogical(
//            new System.Windows.Point(deviceRectangle.Right, deviceRectangle.Bottom),
//            dpiScaleX,
//            dpiScaleY
//        );

//        return new Rect(topLeft, bottomRight);
//    }

//    public static System.Windows.Size LogicalSizeToDevice(System.Windows.Size logicalSize, double dpiScaleX, double dpiScaleY)
//    {
//        System.Windows.Point pt = LogicalPixelsToDevice(
//            new System.Windows.Point(logicalSize.Width, logicalSize.Height),
//            dpiScaleX,
//            dpiScaleY
//        );

//        return new System.Windows.Size { Width = pt.X, Height = pt.Y };
//    }

//    public static System.Windows.Size DeviceSizeToLogical(System.Windows.Size deviceSize, double dpiScaleX, double dpiScaleY)
//    {
//        System.Windows.Point pt = DevicePixelsToLogical(
//            new System.Windows.Point(deviceSize.Width, deviceSize.Height),
//            dpiScaleX,
//            dpiScaleY
//        );

//        return new System.Windows.Size(pt.X, pt.Y);
//    }

//    public static Thickness LogicalThicknessToDevice(
//        Thickness logicalThickness,
//        double dpiScaleX,
//        double dpiScaleY
//    )
//    {
//        System.Windows.Point topLeft = LogicalPixelsToDevice(
//            new System.Windows.Point(logicalThickness.Left, logicalThickness.Top),
//            dpiScaleX,
//            dpiScaleY
//        );
//        System.Windows.Point bottomRight = LogicalPixelsToDevice(
//            new System.Windows.Point(logicalThickness.Right, logicalThickness.Bottom),
//            dpiScaleX,
//            dpiScaleY
//        );

//        return new Thickness(topLeft.X, topLeft.Y, bottomRight.X, bottomRight.Y);
//    }
//}

//public readonly struct DisplayDpi
//{
//    /// <summary>
//    /// Initializes a new instance of the <see cref="DisplayDpi"/> structure.
//    /// </summary>
//    /// <param name="dpiScaleX">The DPI scale on the X axis.</param>
//    /// <param name="dpiScaleY">The DPI scale on the Y axis.</param>
//    public DisplayDpi(double dpiScaleX, double dpiScaleY)
//    {
//        DpiScaleX = dpiScaleX;
//        DpiScaleY = dpiScaleY;

//        DpiX = (int)Math.Round(DpiHelper.DefaultDpi * dpiScaleX, MidpointRounding.AwayFromZero);
//        DpiY = (int)Math.Round(DpiHelper.DefaultDpi * dpiScaleY, MidpointRounding.AwayFromZero);
//    }

//    /// <summary>
//    /// Initializes a new instance of the <see cref="DisplayDpi"/> structure.
//    /// </summary>
//    /// <param name="dpiX">The DPI on the X axis.</param>
//    /// <param name="dpiY">The DPI on the Y axis.</param>
//    public DisplayDpi(int dpiX, int dpiY)
//    {
//        DpiX = dpiX;
//        DpiY = dpiY;

//        DpiScaleX = dpiX / (double)DpiHelper.DefaultDpi;
//        DpiScaleY = dpiY / (double)DpiHelper.DefaultDpi;
//    }

//    /// <summary>
//    /// Gets the DPI on the X axis.
//    /// </summary>
//    public int DpiX { get; }

//    /// <summary>
//    /// Gets the DPI on the Y axis.
//    /// </summary>
//    public int DpiY { get; }

//    /// <summary>
//    /// Gets the DPI scale on the X axis.
//    /// </summary>
//    public double DpiScaleX { get; }

//    /// <summary>
//    /// Gets the DPI scale on the Y axis.
//    /// </summary>
//    public double DpiScaleY { get; }
//}



//public static class UnsafeNativeMethods
//{
//    /// <summary>
//    /// Tries to set the <see cref="Window"/> corner preference.
//    /// </summary>
//    /// <param name="window">Selected window.</param>
//    /// <param name="cornerPreference">Window corner preference.</param>
//    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
//    public static bool ApplyWindowCornerPreference(Window window, WindowCornerPreference cornerPreference) =>
//        GetHandle(window, out IntPtr windowHandle)
//        && ApplyWindowCornerPreference(windowHandle, cornerPreference);

//    /// <summary>
//    /// Tries to set the corner preference of the selected window.
//    /// </summary>
//    /// <param name="handle">Selected window handle.</param>
//    /// <param name="cornerPreference">Window corner preference.</param>
//    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
//    public static bool ApplyWindowCornerPreference(IntPtr handle, WindowCornerPreference cornerPreference)
//    {
//        if (handle == IntPtr.Zero)
//        {
//            return false;
//        }

//        if (!User32.IsWindow(handle))
//        {
//            return false;
//        }

//        int pvAttribute = (int)UnsafeReflection.Cast(cornerPreference);

//        // TODO: Validate HRESULT
//        _ = Dwmapi.DwmSetWindowAttribute(
//            handle,
//            Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE,
//            ref pvAttribute,
//            Marshal.SizeOf(typeof(int))
//        );

//        return true;
//    }

//    /// <summary>
//    /// Tries to remove ImmersiveDarkMode effect from the <see cref="Window"/>.
//    /// </summary>
//    /// <param name="window">The window to which the effect is to be applied.</param>
//    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
//    public static bool RemoveWindowDarkMode(Window? window) =>
//        GetHandle(window, out IntPtr windowHandle) && RemoveWindowDarkMode(windowHandle);

//    /// <summary>
//    /// Tries to remove ImmersiveDarkMode effect from the window handle.
//    /// </summary>
//    /// <param name="handle">Window handle.</param>
//    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
//    public static bool RemoveWindowDarkMode(IntPtr handle)
//    {
//        if (handle == IntPtr.Zero)
//        {
//            return false;
//        }

//        if (!User32.IsWindow(handle))
//        {
//            return false;
//        }

//        var pvAttribute = 0x0; // Disable
//        Dwmapi.DWMWINDOWATTRIBUTE dwAttribute = Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE;

//        if (!Win32.Utilities.IsOSWindows11Insider1OrNewer)
//        {
//            dwAttribute = Dwmapi.DWMWINDOWATTRIBUTE.DMWA_USE_IMMERSIVE_DARK_MODE_OLD;
//        }

//        // TODO: Validate HRESULT
//        _ = Dwmapi.DwmSetWindowAttribute(handle, dwAttribute, ref pvAttribute, Marshal.SizeOf(typeof(int)));

//        return true;
//    }

//    /// <summary>
//    /// Tries to apply ImmersiveDarkMode effect for the <see cref="Window"/>.
//    /// </summary>
//    /// <param name="window">The window to which the effect is to be applied.</param>
//    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
//    public static bool ApplyWindowDarkMode(Window? window) =>
//        GetHandle(window, out IntPtr windowHandle) && ApplyWindowDarkMode(windowHandle);

//    /// <summary>
//    /// Tries to apply ImmersiveDarkMode effect for the window handle.
//    /// </summary>
//    /// <param name="handle">Window handle.</param>
//    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
//    public static bool ApplyWindowDarkMode(IntPtr handle)
//    {
//        if (handle == IntPtr.Zero)
//        {
//            return false;
//        }

//        if (!User32.IsWindow(handle))
//        {
//            return false;
//        }

//        var pvAttribute = 0x1; // Enable
//        Dwmapi.DWMWINDOWATTRIBUTE dwAttribute = Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_USE_IMMERSIVE_DARK_MODE;

//        if (!Win32.Utilities.IsOSWindows11Insider1OrNewer)
//        {
//            dwAttribute = Dwmapi.DWMWINDOWATTRIBUTE.DMWA_USE_IMMERSIVE_DARK_MODE_OLD;
//        }

//        // TODO: Validate HRESULT
//        _ = Dwmapi.DwmSetWindowAttribute(handle, dwAttribute, ref pvAttribute, Marshal.SizeOf(typeof(int)));

//        return true;
//    }

//    /// <summary>
//    /// Tries to remove titlebar from selected <see cref="Window"/>.
//    /// </summary>
//    /// <param name="window">The window to which the effect is to be applied.</param>
//    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
//    public static bool RemoveWindowTitlebarContents(Window? window)
//    {
//        if (window == null)
//        {
//            return false;
//        }

//        if (window.IsLoaded)
//        {
//            return GetHandle(window, out IntPtr windowHandle) && RemoveWindowTitlebarContents(windowHandle);
//        }

//        window.Loaded += (sender, _1) =>
//        {
//            _ = GetHandle(sender as Window, out IntPtr windowHandle);
//            _ = RemoveWindowTitlebarContents(windowHandle);
//        };

//        return true;
//    }

//    /// <summary>
//    /// Tries to remove titlebar from selected window handle.
//    /// </summary>
//    /// <param name="handle">Window handle.</param>
//    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
//    public static bool RemoveWindowTitlebarContents(IntPtr handle)
//    {
//        if (handle == IntPtr.Zero)
//        {
//            return false;
//        }

//        if (!User32.IsWindow(handle))
//        {
//            return false;
//        }

//        var windowStyleLong = User32.GetWindowLong(handle, User32.GWL.GWL_STYLE);
//        windowStyleLong &= ~(int)User32.WS.SYSMENU;

//        IntPtr result = SetWindowLong(handle, User32.GWL.GWL_STYLE, windowStyleLong);

//        return result.ToInt64() > 0x0;
//    }

//    /// <summary>
//    /// Tries to apply selected backdrop type for window handle.
//    /// </summary>
//    /// <param name="handle">Selected window handle.</param>
//    /// <param name="backgroundType">Backdrop type.</param>
//    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
//    public static bool ApplyWindowBackdrop(IntPtr handle, WindowBackdropType backgroundType)
//    {
//        if (handle == IntPtr.Zero)
//        {
//            return false;
//        }

//        if (!User32.IsWindow(handle))
//        {
//            return false;
//        }

//        var backdropPvAttribute = (int)UnsafeReflection.Cast(backgroundType);

//        if (backdropPvAttribute == (int)Dwmapi.DWMSBT.DWMSBT_DISABLE)
//        {
//            return false;
//        }

//        // TODO: Validate HRESULT
//        _ = Dwmapi.DwmSetWindowAttribute(
//            handle,
//            Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
//            ref backdropPvAttribute,
//            Marshal.SizeOf(typeof(int))
//        );

//        return true;
//    }

//    /// <summary>
//    /// Tries to determine whether the provided <see cref="Window"/> has applied legacy backdrop effect.
//    /// </summary>
//    /// <param name="handle">Window handle.</param>
//    /// <param name="backdropType">Background backdrop type.</param>
//    public static bool IsWindowHasBackdrop(IntPtr handle, WindowBackdropType backdropType)
//    {
//        if (!User32.IsWindow(handle))
//        {
//            return false;
//        }

//        var pvAttribute = 0x0;

//        _ = Dwmapi.DwmGetWindowAttribute(
//            handle,
//            Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE,
//            ref pvAttribute,
//            Marshal.SizeOf(typeof(int))
//        );

//        return pvAttribute == (int)UnsafeReflection.Cast(backdropType);
//    }

//    /// <summary>
//    /// Tries to determine whether the provided <see cref="Window"/> has applied legacy Mica effect.
//    /// </summary>
//    /// <param name="window">Window to check.</param>
//    public static bool IsWindowHasLegacyMica(Window? window) =>
//        GetHandle(window, out IntPtr windowHandle) && IsWindowHasLegacyMica(windowHandle);

//    /// <summary>
//    /// Tries to determine whether the provided handle has applied legacy Mica effect.
//    /// </summary>
//    /// <param name="handle">Window handle.</param>
//    public static bool IsWindowHasLegacyMica(IntPtr handle)
//    {
//        if (!User32.IsWindow(handle))
//        {
//            return false;
//        }

//        var pvAttribute = 0x0;

//        _ = Dwmapi.DwmGetWindowAttribute(
//            handle,
//            Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_MICA_EFFECT,
//            ref pvAttribute,
//            Marshal.SizeOf(typeof(int))
//        );

//        return pvAttribute == 0x1;
//    }

//    /// <summary>
//    /// Tries to apply legacy Mica effect for the selected <see cref="Window"/>.
//    /// </summary>
//    /// <param name="window">The window to which the effect is to be applied.</param>
//    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
//    public static bool ApplyWindowLegacyMicaEffect(Window? window) =>
//        GetHandle(window, out IntPtr windowHandle) && ApplyWindowLegacyMicaEffect(windowHandle);

//    /// <summary>
//    /// Tries to apply legacy Mica effect for the selected <see cref="Window"/>.
//    /// </summary>
//    /// <param name="handle">Window handle.</param>
//    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
//    public static bool ApplyWindowLegacyMicaEffect(IntPtr handle)
//    {
//        var backdropPvAttribute = 0x1; // Enable

//        // TODO: Validate HRESULT
//        _ = Dwmapi.DwmSetWindowAttribute(
//            handle,
//            Dwmapi.DWMWINDOWATTRIBUTE.DWMWA_MICA_EFFECT,
//            ref backdropPvAttribute,
//            Marshal.SizeOf(typeof(int))
//        );

//        return true;
//    }

//    /// <summary>
//    /// Tries to apply legacy Acrylic effect for the selected <see cref="Window"/>.
//    /// </summary>
//    /// <param name="window">The window to which the effect is to be applied.</param>
//    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
//    public static bool ApplyWindowLegacyAcrylicEffect(Window? window) =>
//        GetHandle(window, out IntPtr windowHandle) && ApplyWindowLegacyAcrylicEffect(windowHandle);

//    /// <summary>
//    /// Tries to apply legacy Acrylic effect for the selected <see cref="Window"/>.
//    /// </summary>
//    /// <param name="handle">Window handle</param>
//    /// <returns><see langword="true"/> if invocation of native Windows function succeeds.</returns>
//    public static bool ApplyWindowLegacyAcrylicEffect(IntPtr handle)
//    {
//        var accentPolicy = new Interop.User32.ACCENT_POLICY
//        {
//            nAccentState = User32.ACCENT_STATE.ACCENT_ENABLE_ACRYLICBLURBEHIND,
//            nColor = 0x990000 & 0xFFFFFF
//        };

//        int accentStructSize = Marshal.SizeOf(accentPolicy);
//        IntPtr accentPtr = Marshal.AllocHGlobal(accentStructSize);

//        Marshal.StructureToPtr(accentPolicy, accentPtr, false);

//        var data = new User32.WINCOMPATTRDATA
//        {
//            Attribute = User32.WCA.WCA_ACCENT_POLICY,
//            SizeOfData = accentStructSize,
//            Data = accentPtr
//        };

//        _ = User32.SetWindowCompositionAttribute(handle, ref data);

//        Marshal.FreeHGlobal(accentPtr);

//        return true;
//    }

//    /// <summary>
//    /// Tries to get currently selected Window accent color.
//    /// </summary>
//    public static Color GetDwmColor()
//    {
//        try
//        {
//            Dwmapi.DwmGetColorizationParameters(out Dwmapi.DWMCOLORIZATIONPARAMS dwmParams);
//            var values = BitConverter.GetBytes(dwmParams.clrColor);

//            return Color.FromArgb(255, values[2], values[1], values[0]);
//        }
//        catch
//        {
//            var colorizationColorValue = Registry.GetValue(
//                @"HKEY_CURRENT_USER\Software\Microsoft\Windows\DWM",
//                "ColorizationColor",
//                null
//            );

//            if (colorizationColorValue is not null)
//            {
//                try
//                {
//                    var colorizationColor = (uint)(int)colorizationColorValue;
//                    var values = BitConverter.GetBytes(colorizationColor);

//                    return Color.FromArgb(255, values[2], values[1], values[0]);
//                }
//                catch { }
//            }
//        }

//        return GetDefaultWindowsAccentColor();
//    }

//    /// <summary>
//    /// Tries to set taskbar state for the selected window handle.
//    /// </summary>
//    /// <param name="hWnd">Window handle.</param>
//    /// <param name="taskbarFlag">Taskbar flag.</param>
//    internal static bool SetTaskbarState(IntPtr hWnd, ShObjIdl.TBPFLAG taskbarFlag)
//    {
//        if (hWnd == IntPtr.Zero)
//        {
//            return false;
//        }

//        if (!User32.IsWindow(hWnd))
//        {
//            return false;
//        }

//        if (new ShObjIdl.CTaskbarList() is not ShObjIdl.ITaskbarList4 taskbarList)
//        {
//            return false;
//        }

//        taskbarList.HrInit();
//        taskbarList.SetProgressState(hWnd, taskbarFlag);

//        return true;
//    }

//    /// <summary>
//    /// Updates the taskbar progress bar value for a window.
//    /// </summary>
//    /// <param name="hWnd">The handle to the window.</param>
//    /// <param name="taskbarFlag">Progress state flag (paused, etc).</param>
//    /// <param name="current">Current progress value.</param>
//    /// <param name="total">Maximum progress value.</param>
//    /// <returns>True if successful updated, otherwise false.</returns>
//    internal static bool SetTaskbarValue(IntPtr hWnd, ShObjIdl.TBPFLAG taskbarFlag, int current, int total)
//    {
//        if (hWnd == IntPtr.Zero)
//        {
//            return false;
//        }

//        if (!User32.IsWindow(hWnd))
//        {
//            return false;
//        }

//        /* TODO: Get existing taskbar class */

//        if (new ShObjIdl.CTaskbarList() is not ShObjIdl.ITaskbarList4 taskbarList)
//        {
//            return false;
//        }

//        taskbarList.HrInit();
//        taskbarList.SetProgressState(hWnd, taskbarFlag);

//        if (taskbarFlag is not ShObjIdl.TBPFLAG.TBPF_INDETERMINATE and not ShObjIdl.TBPFLAG.TBPF_NOPROGRESS)
//        {
//            taskbarList.SetProgressValue(hWnd, Convert.ToUInt64(current), Convert.ToUInt64(total));
//        }

//        return true;
//    }

//    public static bool RemoveWindowCaption(Window window)
//    {
//        if (window is null)
//        {
//            return false;
//        }

//        IntPtr windowHandle = new WindowInteropHelper(window).Handle;

//        return RemoveWindowCaption(windowHandle);
//    }

//    public static bool RemoveWindowCaption(IntPtr hWnd)
//    {
//        if (hWnd == IntPtr.Zero)
//        {
//            return false;
//        }

//        if (!User32.IsWindow(hWnd))
//        {
//            return false;
//        }

//        var wtaOptions = new UxTheme.WTA_OPTIONS()
//        {
//            dwFlags = UxTheme.WTNCA.NODRAWCAPTION,
//            dwMask = UxTheme.WTNCA.VALIDBITS
//        };

//        UxTheme.SetWindowThemeAttribute(
//            hWnd,
//            UxTheme.WINDOWTHEMEATTRIBUTETYPE.WTA_NONCLIENT,
//            ref wtaOptions,
//            (uint)Marshal.SizeOf(typeof(UxTheme.WTA_OPTIONS))
//        );

//        return true;
//    }

//    public static bool ExtendClientAreaIntoTitleBar(Window window)
//    {
//        if (window is null)
//        {
//            return false;
//        }

//        IntPtr windowHandle = new WindowInteropHelper(window).Handle;

//        return ExtendClientAreaIntoTitleBar(windowHandle);
//    }

//    public static bool ExtendClientAreaIntoTitleBar(IntPtr hWnd)
//    {
//        /*
//         * !! EXPERIMENTAl !!
//         * NOTE: WinRt has ExtendContentIntoTitlebar, but it needs some digging
//         */

//        if (hWnd == IntPtr.Zero)
//        {
//            return false;
//        }

//        if (!User32.IsWindow(hWnd))
//        {
//            return false;
//        }

//        // #1 Remove titlebar elements
//        var wtaOptions = new UxTheme.WTA_OPTIONS()
//        {
//            dwFlags = UxTheme.WTNCA.NODRAWCAPTION | UxTheme.WTNCA.NODRAWICON | UxTheme.WTNCA.NOSYSMENU,
//            dwMask = UxTheme.WTNCA.VALIDBITS
//        };

//        Interop.UxTheme.SetWindowThemeAttribute(
//            hWnd,
//            UxTheme.WINDOWTHEMEATTRIBUTETYPE.WTA_NONCLIENT,
//            ref wtaOptions,
//            (uint)Marshal.SizeOf(typeof(UxTheme.WTA_OPTIONS))
//        );

//        DisplayDpi windowDpi = DpiHelper.GetWindowDpi(hWnd);

//        // #2 Extend glass frame
//        Thickness deviceGlassThickness = DpiHelper.LogicalThicknessToDevice(
//            new Thickness(-1, -1, -1, -1),
//            windowDpi.DpiScaleX,
//            windowDpi.DpiScaleY
//        );

//        var dwmMargin = new UxTheme.MARGINS
//        {
//            // err on the side of pushing in glass an extra pixel.
//            cxLeftWidth = (int)Math.Ceiling(deviceGlassThickness.Left),
//            cxRightWidth = (int)Math.Ceiling(deviceGlassThickness.Right),
//            cyTopHeight = (int)Math.Ceiling(deviceGlassThickness.Top),
//            cyBottomHeight = (int)Math.Ceiling(deviceGlassThickness.Bottom),
//        };

//        // #3 Extend client area
//        Interop.Dwmapi.DwmExtendFrameIntoClientArea(hWnd, ref dwmMargin);

//        // #4 Clear rounding region
//        Interop.User32.SetWindowRgn(hWnd, IntPtr.Zero, Interop.User32.IsWindowVisible(hWnd));

//        return true;
//    }

//    /// <summary>
//    /// Checks whether the DWM composition is enabled.
//    /// </summary>
//    public static bool IsCompositionEnabled()
//    {
//        _ = Dwmapi.DwmIsCompositionEnabled(out var isEnabled);

//        return isEnabled == 0x1;
//    }

//    /// <summary>
//    /// Checks if provided pointer represents existing window.
//    /// </summary>
//    public static bool IsValidWindow(IntPtr hWnd)
//    {
//        return User32.IsWindow(hWnd);
//    }

//    /// <summary>
//    /// Tries to get the pointer to the window handle.
//    /// </summary>
//    /// <returns><see langword="true"/> if the handle is not <see cref="IntPtr.Zero"/>.</returns>
//    private static bool GetHandle(Window? window, out IntPtr windowHandle)
//    {
//        if (window is null)
//        {
//            windowHandle = IntPtr.Zero;

//            return false;
//        }

//        windowHandle = new WindowInteropHelper(window).Handle;

//        return windowHandle != IntPtr.Zero;
//    }

//    private static IntPtr SetWindowLong(IntPtr handle, User32.GWL nIndex, long windowStyleLong)
//    {
//        if (IntPtr.Size == 4)
//        {
//            return new IntPtr(User32.SetWindowLong(handle, (int)nIndex, (int)windowStyleLong));
//        }

//        return User32.SetWindowLongPtr(handle, (int)nIndex, checked((IntPtr)windowStyleLong));
//    }

//    private static Color GetDefaultWindowsAccentColor()
//    {
//        // Windows default accent color
//        // https://learn.microsoft.com/windows-hardware/customize/desktop/unattend/microsoft-windows-shell-setup-themes-windowcolor#values
//        return Color.FromArgb(0xff, 0x00, 0x78, 0xd7);
//    }
//}

//// This Source Code Form is subject to the terms of the MIT License.
//// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
//// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
//// All Rights Reserved.

//// This Source Code is partially based on reverse engineering of the Windows Operating System,
//// and is intended for use on Windows systems only.
//// This Source Code is partially based on the source code provided by the .NET Foundation.
////
//// NOTE:
//// I split unmanaged code stuff into the NativeMethods library.
//// If you have suggestions for the code below, please submit your changes there.
//// https://github.com/lepoco/nativemethods
////
//// Windows Kits\10\Include\10.0.22000.0\um\dwmapi.h

//// ReSharper disable IdentifierTypo
//// ReSharper disable InconsistentNaming
//#pragma warning disable SA1307 // Accessible fields should begin with upper-case letter

//using System.Runtime.InteropServices;

//namespace Wpf.Ui.Interop;

///// <summary>
///// Desktop Window Manager (DWM).
///// </summary>
//internal static class Dwmapi
//{
//    /// <summary>
//    /// Cloaked flags describing why a window is cloaked.
//    /// </summary>
//    public enum DWM_CLOAKED
//    {
//        DWM_CLOAKED_APP = 0x00000001,
//        DWM_CLOAKED_SHELL = 0x00000002,
//        DWM_CLOAKED_INHERITED = 0x00000004
//    }

//    /// <summary>
//    /// GT_*
//    /// </summary>
//    public enum GESTURE_TYPE
//    {
//        GT_PEN_TAP = 0,
//        GT_PEN_DOUBLETAP = 1,
//        GT_PEN_RIGHTTAP = 2,
//        GT_PEN_PRESSANDHOLD = 3,
//        GT_PEN_PRESSANDHOLDABORT = 4,
//        GT_TOUCH_TAP = 5,
//        GT_TOUCH_DOUBLETAP = 6,
//        GT_TOUCH_RIGHTTAP = 7,
//        GT_TOUCH_PRESSANDHOLD = 8,
//        GT_TOUCH_PRESSANDHOLDABORT = 9,
//        GT_TOUCH_PRESSANDTAP = 10,
//    }

//    /// <summary>
//    /// DWMTWR_* Tab window requirements.
//    /// </summary>
//    public enum DWM_TAB_WINDOW_REQUIREMENTS
//    {
//        /// <summary>
//        /// This result means the window meets all requirements requested.
//        /// </summary>
//        DWMTWR_NONE = 0x0000,

//        /// <summary>
//        /// In some configurations, admin/user setting or mode of the system means that windows won't be tabbed
//        /// This requirement says that the system/mode must implement tabbing and if it does not
//        /// nothing can be done to change this.
//        /// </summary>
//        DWMTWR_IMPLEMENTED_BY_SYSTEM = 0x0001,

//        /// <summary>
//        /// The window has an owner or parent so is ineligible for tabbing.
//        /// </summary>
//        DWMTWR_WINDOW_RELATIONSHIP = 0x0002,

//        /// <summary>
//        /// The window has styles that make it ineligible for tabbing.
//        /// <para>To be eligible windows must:</para>
//        /// <para>Have the WS_OVERLAPPEDWINDOW (WS_CAPTION, WS_THICKFRAME, etc.) styles set.</para>
//        /// <para>Not have WS_POPUP, WS_CHILD or WS_DLGFRAME set.</para>
//        /// <para>Not have WS_EX_TOPMOST or WS_EX_TOOLWINDOW set.</para>
//        /// </summary>
//        DWMTWR_WINDOW_STYLES = 0x0004,

//        // The window has a region (set using SetWindowRgn) making it ineligible.
//        DWMTWR_WINDOW_REGION = 0x0008,

//        /// <summary>
//        /// The window is ineligible due to its Dwm configuration.
//        /// It must not extended its client area into the title bar using DwmExtendFrameIntoClientArea
//        /// It must not have DWMWA_NCRENDERING_POLICY set to DWMNCRP_ENABLED
//        /// </summary>
//        DWMTWR_WINDOW_DWM_ATTRIBUTES = 0x0010,

//        /// <summary>
//        /// The window is ineligible due to it's margins, most likely due to custom handling in WM_NCCALCSIZE.
//        /// The window must use the default window margins for the non-client area.
//        /// </summary>
//        DWMTWR_WINDOW_MARGINS = 0x0020,

//        /// <summary>
//        /// The window has been explicitly opted out by setting DWMWA_TABBING_ENABLED to FALSE.
//        /// </summary>
//        DWMTWR_TABBING_ENABLED = 0x0040,

//        /// <summary>
//        /// The user has configured this application to not participate in tabbing.
//        /// </summary>
//        DWMTWR_USER_POLICY = 0x0080,

//        /// <summary>
//        /// The group policy has configured this application to not participate in tabbing.
//        /// </summary>
//        DWMTWR_GROUP_POLICY = 0x0100,

//        /// <summary>
//        /// This is set if app compat has blocked tabs for this window. Can be overridden per window by setting
//        /// DWMWA_TABBING_ENABLED to TRUE. That does not override any other tabbing requirements.
//        /// </summary>
//        DWMTWR_APP_COMPAT = 0x0200
//    }

//    /// <summary>
//    /// Flags used by the DwmSetWindowAttribute function to specify the rounded corner preference for a window.
//    /// </summary>
//    [Flags]
//    public enum DWM_WINDOW_CORNER_PREFERENCE
//    {
//        DEFAULT = 0,
//        DONOTROUND = 1,
//        ROUND = 2,
//        ROUNDSMALL = 3
//    }

//    /// <summary>
//    /// Backdrop types.
//    /// </summary>
//    [Flags]
//    public enum DWMSBT : uint
//    {
//        /// <summary>
//        /// Automatically selects backdrop effect.
//        /// </summary>
//        DWMSBT_AUTO = 0,

//        /// <summary>
//        /// Turns off the backdrop effect.
//        /// </summary>
//        DWMSBT_DISABLE = 1,

//        /// <summary>
//        /// Sets Mica effect with generated wallpaper tint.
//        /// </summary>
//        DWMSBT_MAINWINDOW = 2,

//        /// <summary>
//        /// Sets acrlic effect.
//        /// </summary>
//        DWMSBT_TRANSIENTWINDOW = 3,

//        /// <summary>
//        /// Sets blurred wallpaper effect, like Mica without tint.
//        /// </summary>
//        DWMSBT_TABBEDWINDOW = 4
//    }

//    /// <summary>
//    /// Non-client rendering policy attribute values
//    /// </summary>
//    public enum DWMNCRENDERINGPOLICY
//    {
//        /// <summary>
//        /// Enable/disable non-client rendering based on window style
//        /// </summary>
//        DWMNCRP_USEWINDOWSTYLE,

//        /// <summary>
//        /// Disabled non-client rendering; window style is ignored
//        /// </summary>
//        DWMNCRP_DISABLED,

//        /// <summary>
//        /// Enabled non-client rendering; window style is ignored
//        /// </summary>
//        DWMNCRP_ENABLED,

//        /// <summary>
//        /// Sentinel value.
//        /// </summary>
//        DWMNCRP_LAST
//    }

//    /// <summary>
//    /// Values designating how Flip3D treats a given window.
//    /// </summary>
//    public enum DWMFLIP3DWINDOWPOLICY
//    {
//        /// <summary>
//        /// Hide or include the window in Flip3D based on window style and visibility.
//        /// </summary>
//        DWMFLIP3D_DEFAULT,

//        /// <summary>
//        /// Display the window under Flip3D and disabled.
//        /// </summary>
//        DWMFLIP3D_EXCLUDEBELOW,

//        /// <summary>
//        /// Display the window above Flip3D and enabled.
//        /// </summary>
//        DWMFLIP3D_EXCLUDEABOVE,

//        /// <summary>
//        /// Sentinel value.
//        /// </summary>
//        DWMFLIP3D_LAST
//    }

//    /// <summary>
//    /// Options used by the DwmGetWindowAttribute and DwmSetWindowAttribute functions.
//    /// <para><see href="https://github.com/electron/electron/issues/29937"/></para>
//    /// </summary>
//    [Flags]
//    public enum DWMWINDOWATTRIBUTE
//    {
//        /// <summary>
//        /// Is non-client rendering enabled/disabled
//        /// </summary>
//        DWMWA_NCRENDERING_ENABLED = 1,

//        /// <summary>
//        /// DWMNCRENDERINGPOLICY - Non-client rendering policy
//        /// </summary>
//        DWMWA_NCRENDERING_POLICY = 2,

//        /// <summary>
//        /// Potentially enable/forcibly disable transitions
//        /// </summary>
//        DWMWA_TRANSITIONS_FORCEDISABLED = 3,

//        /// <summary>
//        /// Enables content rendered in the non-client area to be visible on the frame drawn by DWM.
//        /// </summary>
//        DWMWA_ALLOW_NCPAINT = 4,

//        /// <summary>
//        /// Retrieves the bounds of the caption button area in the window-relative space.
//        /// </summary>
//        DWMWA_CAPTION_BUTTON_BOUNDS = 5,

//        /// <summary>
//        /// Is non-client content RTL mirrored
//        /// </summary>
//        DWMWA_NONCLIENT_RTL_LAYOUT = 6,

//        /// <summary>
//        /// Forces the window to display an iconic thumbnail or peek representation (a static bitmap), even if a live or snapshot representation of the window is available.
//        /// </summary>
//        DWMWA_FORCE_ICONIC_REPRESENTATION = 7,

//        /// <summary>
//        /// Designates how Flip3D will treat the window.
//        /// </summary>
//        DWMWA_FLIP3D_POLICY = 8,

//        /// <summary>
//        /// Gets the extended frame bounds rectangle in screen space
//        /// </summary>
//        DWMWA_EXTENDED_FRAME_BOUNDS = 9,

//        /// <summary>
//        /// Indicates an available bitmap when there is no better thumbnail representation.
//        /// </summary>
//        DWMWA_HAS_ICONIC_BITMAP = 10,

//        /// <summary>
//        /// Don't invoke Peek on the window.
//        /// </summary>
//        DWMWA_DISALLOW_PEEK = 11,

//        /// <summary>
//        /// LivePreview exclusion information
//        /// </summary>
//        DWMWA_EXCLUDED_FROM_PEEK = 12,

//        /// <summary>
//        /// Cloaks the window such that it is not visible to the user.
//        /// </summary>
//        DWMWA_CLOAK = 13,

//        /// <summary>
//        /// If the window is cloaked, provides one of the following values explaining why.
//        /// </summary>
//        DWMWA_CLOAKED = 14,

//        /// <summary>
//        /// Freeze the window's thumbnail image with its current visuals. Do no further live updates on the thumbnail image to match the window's contents.
//        /// </summary>
//        DWMWA_FREEZE_REPRESENTATION = 15,

//        /// <summary>
//        /// BOOL, Updates the window only when desktop composition runs for other reasons
//        /// </summary>
//        DWMWA_PASSIVE_UPDATE_MODE = 16,

//        /// <summary>
//        /// BOOL, Allows the use of host backdrop brushes for the window.
//        /// </summary>
//        DWMWA_USE_HOSTBACKDROPBRUSH = 17,

//        /// <summary>
//        /// Allows a window to either use the accent color, or dark, according to the user Color Mode preferences.
//        /// </summary>
//        DMWA_USE_IMMERSIVE_DARK_MODE_OLD = 19,

//        /// <summary>
//        /// Allows a window to either use the accent color, or dark, according to the user Color Mode preferences.
//        /// </summary>
//        DWMWA_USE_IMMERSIVE_DARK_MODE = 20,

//        /// <summary>
//        /// Controls the policy that rounds top-level window corners.
//        /// <para>Windows 11 and above.</para>
//        /// </summary>
//        DWMWA_WINDOW_CORNER_PREFERENCE = 33,

//        /// <summary>
//        /// The color of the thin border around a top-level window.
//        /// </summary>
//        DWMWA_BORDER_COLOR = 34,

//        /// <summary>
//        /// The color of the caption.
//        /// <para>Windows 11 and above.</para>
//        /// </summary>
//        DWMWA_CAPTION_COLOR = 35,

//        /// <summary>
//        /// The color of the caption text.
//        /// <para>Windows 11 and above.</para>
//        /// </summary>
//        DWMWA_TEXT_COLOR = 36,

//        /// <summary>
//        /// Width of the visible border around a thick frame window.
//        /// <para>Windows 11 and above.</para>
//        /// </summary>
//        DWMWA_VISIBLE_FRAME_BORDER_THICKNESS = 37,

//        /// <summary>
//        /// Allows to enter a value from 0 to 4 deciding on the imposed backdrop effect.
//        /// </summary>
//        DWMWA_SYSTEMBACKDROP_TYPE = 38,

//        /// <summary>
//        /// Indicates whether the window should use the Mica effect.
//        /// <para>Windows 11 and above.</para>
//        /// </summary>
//        DWMWA_MICA_EFFECT = 1029
//    }

//    /// <summary>
//    /// Represents the current DWM color accent settings.
//    /// </summary>
//    [StructLayout(LayoutKind.Sequential)]
//    public struct DWMCOLORIZATIONPARAMS
//    {
//        /// <summary>
//        /// ColorizationColor
//        /// </summary>
//        public uint clrColor;

//        /// <summary>
//        /// ColorizationAfterglow.
//        /// </summary>
//        public uint clrAfterGlow;

//        /// <summary>
//        /// ColorizationColorBalance.
//        /// </summary>
//        public uint nIntensity;

//        /// <summary>
//        /// ColorizationAfterglowBalance.
//        /// </summary>
//        public uint clrAfterGlowBalance;

//        /// <summary>
//        /// ColorizationBlurBalance.
//        /// </summary>
//        public uint clrBlurBalance;

//        /// <summary>
//        /// ColorizationGlassReflectionIntensity.
//        /// </summary>
//        public uint clrGlassReflectionIntensity;

//        /// <summary>
//        /// ColorizationOpaqueBlend.
//        /// </summary>
//        public bool fOpaque;
//    }

//    /// <summary>
//    /// Defines a data type used by the Desktop Window Manager (DWM) APIs. It represents a generic ratio and is used for different purposes and units even within a single API.
//    /// </summary>
//    [StructLayout(LayoutKind.Sequential)]
//    public struct UNSIGNED_RATIO
//    {
//        /// <summary>
//        /// The ratio numerator.
//        /// </summary>
//        public uint uiNumerator;

//        /// <summary>
//        /// The ratio denominator.
//        /// </summary>
//        public uint uiDenominator;
//    }

//    /// <summary>
//    /// Specifies the input operations for which visual feedback should be provided. This enumeration is used by the DwmShowContact function.
//    /// </summary>
//    public enum DWM_SHOWCONTACT
//    {
//        DWMSC_DOWN,
//        DWMSC_UP,
//        DWMSC_DRAG,
//        DWMSC_HOLD,
//        DWMSC_PENBARREL,
//        DWMSC_NONE,
//        DWMSC_ALL
//    }

//    /// <summary>
//    /// Flags used by the DwmSetPresentParameters function to specify the frame sampling type.
//    /// </summary>
//    public enum DWM_SOURCE_FRAME_SAMPLING
//    {
//        /// <summary>
//        /// Use the first source frame that includes the first refresh of the output frame
//        /// </summary>
//        DWM_SOURCE_FRAME_SAMPLING_POINT,

//        /// <summary>
//        /// Use the source frame that includes the most refreshes of out the output frame
//        /// in case of multiple source frames with the same coverage the last will be used
//        /// </summary>
//        DWM_SOURCE_FRAME_SAMPLING_COVERAGE,

//        /// <summary>
//        /// Sentinel value.
//        /// </summary>
//        DWM_SOURCE_FRAME_SAMPLING_LAST
//    }

//    /// <summary>
//    /// Specifies Desktop Window Manager (DWM) composition timing information. Used by the <see cref="DwmGetCompositionTimingInfo"/> function.
//    /// </summary>
//    [StructLayout(LayoutKind.Sequential, Pack = 1)]
//    public struct DWM_TIMING_INFO
//    {
//        public int cbSize;
//        public UNSIGNED_RATIO rateRefresh;
//        public ulong qpcRefreshPeriod;
//        public UNSIGNED_RATIO rateCompose;
//        public ulong qpcVBlank;
//        public ulong cRefresh;
//        public uint cDXRefresh;
//        public ulong qpcCompose;
//        public ulong cFrame;
//        public uint cDXPresent;
//        public ulong cRefreshFrame;
//        public ulong cFrameSubmitted;
//        public uint cDXPresentSubmitted;
//        public ulong cFrameConfirmed;
//        public uint cDXPresentConfirmed;
//        public ulong cRefreshConfirmed;
//        public uint cDXRefreshConfirmed;
//        public ulong cFramesLate;
//        public uint cFramesOutstanding;
//        public ulong cFrameDisplayed;
//        public ulong qpcFrameDisplayed;
//        public ulong cRefreshFrameDisplayed;
//        public ulong cFrameComplete;
//        public ulong qpcFrameComplete;
//        public ulong cFramePending;
//        public ulong qpcFramePending;
//        public ulong cFramesDisplayed;
//        public ulong cFramesComplete;
//        public ulong cFramesPending;
//        public ulong cFramesAvailable;
//        public ulong cFramesDropped;
//        public ulong cFramesMissed;
//        public ulong cRefreshNextDisplayed;
//        public ulong cRefreshNextPresented;
//        public ulong cRefreshesDisplayed;
//        public ulong cRefreshesPresented;
//        public ulong cRefreshStarted;
//        public ulong cPixelsReceived;
//        public ulong cPixelsDrawn;
//        public ulong cBuffersEmpty;
//    }

//    /// <summary>
//    /// SIT flags.
//    /// </summary>
//    public enum DWM_SIT
//    {
//        /// <summary>
//        /// None.
//        /// </summary>
//        NONE,

//        /// <summary>
//        /// Displays a frame around the provided bitmap.
//        /// </summary>
//        DISPLAYFRAME = 1,
//    }

//    /// <summary>
//    /// Obtains a value that indicates whether Desktop Window Manager (DWM) composition is enabled.
//    /// </summary>
//    /// <param name="pfEnabled">A pointer to a value that, when this function returns successfully, receives TRUE if DWM composition is enabled; otherwise, FALSE.</param>
//    /// <returns>If this function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
//    [DllImport(Libraries.Dwmapi, BestFitMapping = false)]
//    public static extern int DwmIsCompositionEnabled([Out] out int pfEnabled);

//    /// <summary>
//    /// Extends the window frame into the client area.
//    /// </summary>
//    /// <param name="hWnd">The handle to the window in which the frame will be extended into the client area.</param>
//    /// <param name="pMarInset">A pointer to a MARGINS structure that describes the margins to use when extending the frame into the client area.</param>
//    [DllImport(Libraries.Dwmapi, PreserveSig = false)]
//    public static extern void DwmExtendFrameIntoClientArea(
//        [In] IntPtr hWnd,
//        [In] ref UxTheme.MARGINS pMarInset
//    );

//    /// <summary>
//    /// Retrieves the current composition timing information for a specified window.
//    /// </summary>
//    /// <param name="hWnd">The handle to the window for which the composition timing information should be retrieved.</param>
//    /// <param name="pTimingInfo">A pointer to a <see cref="DWM_TIMING_INFO"/> structure that, when this function returns successfully, receives the current composition timing information for the window.</param>
//    [DllImport(Libraries.Dwmapi)]
//    public static extern void DwmGetCompositionTimingInfo(
//        [In] IntPtr hWnd,
//        [In] ref DWM_TIMING_INFO pTimingInfo
//    );

//    /// <summary>
//    /// Called by an application to indicate that all previously provided iconic bitmaps from a window, both thumbnails and peek representations, should be refreshed.
//    /// </summary>
//    /// <param name="hWnd">A handle to the window or tab whose bitmaps are being invalidated through this call. This window must belong to the calling process.</param>
//    [DllImport(Libraries.Dwmapi, PreserveSig = false)]
//    public static extern void DwmInvalidateIconicBitmaps([In] IntPtr hWnd);

//    /// <summary>
//    /// Sets a static, iconic bitmap on a window or tab to use as a thumbnail representation. The taskbar can use this bitmap as a thumbnail switch target for the window or tab.
//    /// </summary>
//    /// <param name="hWnd">A handle to the window or tab. This window must belong to the calling process.</param>
//    /// <param name="hbmp">A handle to the bitmap to represent the window that hwnd specifies.</param>
//    /// <param name="dwSITFlags">The display options for the thumbnail.</param>
//    [DllImport(Libraries.Dwmapi, PreserveSig = false)]
//    public static extern void DwmSetIconicThumbnail(
//        [In] IntPtr hWnd,
//        [In] IntPtr hbmp,
//        [In] DWM_SIT dwSITFlags
//    );

//    /// <summary>
//    /// Sets a static, iconic bitmap to display a live preview (also known as a Peek preview) of a window or tab. The taskbar can use this bitmap to show a full-sized preview of a window or tab.
//    /// </summary>
//    /// <param name="hWnd">A handle to the window. This window must belong to the calling process.</param>
//    /// <param name="hbmp">A handle to the bitmap to represent the window that hwnd specifies.</param>
//    /// <param name="pptClient">The offset of a tab window's client region (the content area inside the client window frame) from the host window's frame. This offset enables the tab window's contents to be drawn correctly in a live preview when it is drawn without its frame.</param>
//    /// <param name="dwSITFlags">The display options for the live preview.</param>
//    [DllImport(Libraries.Dwmapi, PreserveSig = false)]
//    public static extern int DwmSetIconicLivePreviewBitmap(
//        [In] IntPtr hWnd,
//        [In] IntPtr hbmp,
//        [In, Optional] POINT pptClient,
//        [In] DWM_SIT dwSITFlags
//    );

//    /// <summary>
//    /// Sets the value of Desktop Window Manager (DWM) non-client rendering attributes for a window.
//    /// </summary>
//    /// <param name="hWnd">The handle to the window for which the attribute value is to be set.</param>
//    /// <param name="dwAttribute">A flag describing which value to set, specified as a value of the DWMWINDOWATTRIBUTE enumeration.</param>
//    /// <param name="pvAttribute">A pointer to an object containing the attribute value to set.</param>
//    /// <param name="cbAttribute">The size, in bytes, of the attribute value being set via the <c>pvAttribute</c> parameter.</param>
//    /// <returns>If the function succeeds, it returns <c>S_OK</c>. Otherwise, it returns an <c>HRESULT</c> error code.</returns>
//    [DllImport(Libraries.Dwmapi)]
//    public static extern int DwmSetWindowAttribute(
//        [In] IntPtr hWnd,
//        [In] int dwAttribute,
//        [In] ref int pvAttribute,
//        [In] int cbAttribute
//    );

//    /// <summary>
//    /// Sets the value of Desktop Window Manager (DWM) non-client rendering attributes for a window.
//    /// </summary>
//    /// <param name="hWnd">The handle to the window for which the attribute value is to be set.</param>
//    /// <param name="dwAttribute">A flag describing which value to set, specified as a value of the DWMWINDOWATTRIBUTE enumeration.</param>
//    /// <param name="pvAttribute">A pointer to an object containing the attribute value to set.</param>
//    /// <param name="cbAttribute">The size, in bytes, of the attribute value being set via the <c>pvAttribute</c> parameter.</param>
//    /// <returns>If the function succeeds, it returns <c>S_OK</c>. Otherwise, it returns an <c>HRESULT</c> error code.</returns>
//    [DllImport(Libraries.Dwmapi)]
//    public static extern int DwmSetWindowAttribute(
//        [In] IntPtr hWnd,
//        [In] DWMWINDOWATTRIBUTE dwAttribute,
//        [In] ref int pvAttribute,
//        [In] int cbAttribute
//    );

//    /// <summary>
//    /// Sets the value of Desktop Window Manager (DWM) non-client rendering attributes for a window.
//    /// </summary>
//    /// <param name="hWnd">The handle to the window for which the attribute value is to be set.</param>
//    /// <param name="dwAttribute">A flag describing which value to set, specified as a value of the DWMWINDOWATTRIBUTE enumeration.</param>
//    /// <param name="pvAttribute">A pointer to an object containing the attribute value to set.</param>
//    /// <param name="cbAttribute">The size, in bytes, of the attribute value being set via the <c>pvAttribute</c> parameter.</param>
//    /// <returns>If the function succeeds, it returns <c>S_OK</c>. Otherwise, it returns an <c>HRESULT</c> error code.</returns>
//    [DllImport(Libraries.Dwmapi)]
//    public static extern int DwmSetWindowAttribute(
//        [In] IntPtr hWnd,
//        [In] DWMWINDOWATTRIBUTE dwAttribute,
//        [In] ref uint pvAttribute,
//        [In] int cbAttribute
//    );

//    /// <summary>
//    /// Retrieves the current value of a specified Desktop Window Manager (DWM) attribute applied to a window. For programming guidance, and code examples, see Controlling non-client region rendering.
//    /// </summary>
//    /// <param name="hWnd">The handle to the window from which the attribute value is to be retrieved.</param>
//    /// <param name="dwAttributeToGet">A flag describing which value to retrieve, specified as a value of the <see cref="DWMWINDOWATTRIBUTE"/> enumeration.</param>
//    /// <param name="pvAttributeValue">A pointer to a value which, when this function returns successfully, receives the current value of the attribute. The type of the retrieved value depends on the value of the dwAttribute parameter.</param>
//    /// <param name="cbAttribute">The size, in bytes, of the attribute value being received via the pvAttribute parameter.</param>
//    /// <returns>If the function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
//    [DllImport(Libraries.Dwmapi)]
//    public static extern int DwmGetWindowAttribute(
//        [In] IntPtr hWnd,
//        [In] DWMWINDOWATTRIBUTE dwAttributeToGet,
//        [In] ref int pvAttributeValue,
//        [In] int cbAttribute
//    );

//    /// <summary>
//    /// Retrieves the current value of a specified Desktop Window Manager (DWM) attribute applied to a window. For programming guidance, and code examples, see Controlling non-client region rendering.
//    /// </summary>
//    /// <param name="hWnd">The handle to the window from which the attribute value is to be retrieved.</param>
//    /// <param name="dwAttributeToGet">A flag describing which value to retrieve, specified as a value of the <see cref="DWMWINDOWATTRIBUTE"/> enumeration.</param>
//    /// <param name="pvAttributeValue">A pointer to a value which, when this function returns successfully, receives the current value of the attribute. The type of the retrieved value depends on the value of the dwAttribute parameter.</param>
//    /// <param name="cbAttribute">The size, in bytes, of the attribute value being received via the pvAttribute parameter.</param>
//    /// <returns>If the function succeeds, it returns S_OK. Otherwise, it returns an HRESULT error code.</returns>
//    [DllImport(Libraries.Dwmapi)]
//    public static extern int DwmGetWindowAttribute(
//        [In] IntPtr hWnd,
//        [In] int dwAttributeToGet,
//        [In] ref int pvAttributeValue,
//        [In] int cbAttribute
//    );

//    /// <summary>
//    /// The feature is not included in the Microsoft documentation. Reads Desktop Window Manager (DWM) color information.
//    /// </summary>
//    /// <param name="dwParameters">A pointer to a reference value that will hold the color information.</param>
//    [DllImport(Libraries.Dwmapi, EntryPoint = "#127", PreserveSig = false, CharSet = CharSet.Unicode)]
//    public static extern void DwmGetColorizationParameters([Out] out DWMCOLORIZATIONPARAMS dwParameters);
//}

//#pragma warning restore SA1307 // Accessible fields should begin with upper-case letter


//public class FluentWindow : System.Windows.Window
//{
//    private WindowInteropHelper? _interopHelper = null;

//    /// <summary>
//    /// Gets contains helper for accessing this window handle.
//    /// </summary>
//    protected WindowInteropHelper InteropHelper
//    {
//        get => _interopHelper ??= new WindowInteropHelper(this);
//    }

//    /// <summary>Identifies the <see cref="WindowCornerPreference"/> dependency property.</summary>
//    public static readonly DependencyProperty WindowCornerPreferenceProperty = DependencyProperty.Register(
//        nameof(WindowCornerPreference),
//        typeof(WindowCornerPreference),
//        typeof(FluentWindow),
//        new PropertyMetadata(WindowCornerPreference.Round, OnWindowCornerPreferenceChanged)
//    );

//    /// <summary>Identifies the <see cref="WindowBackdropType"/> dependency property.</summary>
//    public static readonly DependencyProperty WindowBackdropTypeProperty = DependencyProperty.Register(
//        nameof(WindowBackdropType),
//        typeof(WindowBackdropType),
//        typeof(FluentWindow),
//        new PropertyMetadata(WindowBackdropType.None, OnWindowBackdropTypeChanged)
//    );

//    /// <summary>Identifies the <see cref="ExtendsContentIntoTitleBar"/> dependency property.</summary>
//    public static readonly DependencyProperty ExtendsContentIntoTitleBarProperty =
//        DependencyProperty.Register(
//            nameof(ExtendsContentIntoTitleBar),
//            typeof(bool),
//            typeof(FluentWindow),
//            new PropertyMetadata(false, OnExtendsContentIntoTitleBarChanged)
//        );

//    /// <summary>
//    /// Gets or sets a value determining corner preference for current <see cref="Window"/>.
//    /// </summary>
//    public WindowCornerPreference WindowCornerPreference
//    {
//        get => (WindowCornerPreference)GetValue(WindowCornerPreferenceProperty);
//        set => SetValue(WindowCornerPreferenceProperty, value);
//    }

//    /// <summary>
//    /// Gets or sets a value determining preferred backdrop type for current <see cref="Window"/>.
//    /// </summary>
//    public WindowBackdropType WindowBackdropType
//    {
//        get => (WindowBackdropType)GetValue(WindowBackdropTypeProperty);
//        set => SetValue(WindowBackdropTypeProperty, value);
//    }

//    /// <summary>
//    /// Gets or sets a value indicating whether the default title bar of the window should be hidden to create space for app content.
//    /// </summary>
//    public bool ExtendsContentIntoTitleBar
//    {
//        get => (bool)GetValue(ExtendsContentIntoTitleBarProperty);
//        set => SetValue(ExtendsContentIntoTitleBarProperty, value);
//    }

//    /// <summary>
//    /// Initializes a new instance of the <see cref="FluentWindow"/> class.
//    /// </summary>
//    public FluentWindow()
//    {
//        SetResourceReference(StyleProperty, typeof(FluentWindow));
//    }

//    /// <summary>
//    /// Initializes static members of the <see cref="FluentWindow"/> class.
//    /// Overrides default properties.
//    /// </summary>
//    /// <remarks>
//    /// Overrides default properties.
//    /// </remarks>
//    static FluentWindow()
//    {
//        DefaultStyleKeyProperty.OverrideMetadata(
//            typeof(FluentWindow),
//            new FrameworkPropertyMetadata(typeof(FluentWindow))
//        );
//    }

//    /// <inheritdoc />
//    protected override void OnSourceInitialized(EventArgs e)
//    {
//        OnCornerPreferenceChanged(default, WindowCornerPreference);
//        OnExtendsContentIntoTitleBarChanged(default, ExtendsContentIntoTitleBar);
//        OnBackdropTypeChanged(default, WindowBackdropType);

//        base.OnSourceInitialized(e);
//    }

//    /// <summary>
//    /// Private <see cref="WindowCornerPreference"/> property callback.
//    /// </summary>
//    private static void OnWindowCornerPreferenceChanged(
//        DependencyObject d,
//        DependencyPropertyChangedEventArgs e
//    )
//    {
//        if (d is not FluentWindow window)
//        {
//            return;
//        }

//        if (e.OldValue == e.NewValue)
//        {
//            return;
//        }

//        window.OnCornerPreferenceChanged(
//            (WindowCornerPreference)e.OldValue,
//            (WindowCornerPreference)e.NewValue
//        );
//    }

//    /// <summary>
//    /// This virtual method is called when <see cref="WindowCornerPreference"/> is changed.
//    /// </summary>
//    protected virtual void OnCornerPreferenceChanged(
//        WindowCornerPreference oldValue,
//        WindowCornerPreference newValue
//    )
//    {
//        if (InteropHelper.Handle == IntPtr.Zero)
//        {
//            return;
//        }

//        _ = UnsafeNativeMethods.ApplyWindowCornerPreference(InteropHelper.Handle, newValue);
//    }

//    /// <summary>
//    /// Private <see cref="WindowBackdropType"/> property callback.
//    /// </summary>
//    private static void OnWindowBackdropTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
//    {
//        if (d is not FluentWindow window)
//        {
//            return;
//        }

//        if (e.OldValue == e.NewValue)
//        {
//            return;
//        }

//        window.OnBackdropTypeChanged((WindowBackdropType)e.OldValue, (WindowBackdropType)e.NewValue);
//    }

//    /// <summary>
//    /// This virtual method is called when <see cref="WindowBackdropType"/> is changed.
//    /// </summary>
//    protected virtual void OnBackdropTypeChanged(WindowBackdropType oldValue, WindowBackdropType newValue)
//    {
//        if (Appearance.ApplicationThemeManager.GetAppTheme() == Appearance.ApplicationTheme.HighContrast)
//        {
//            newValue = WindowBackdropType.None;
//        }

//        if (InteropHelper.Handle == IntPtr.Zero)
//        {
//            return;
//        }

//        if (newValue == WindowBackdropType.None)
//        {
//            _ = WindowBackdrop.RemoveBackdrop(this);

//            return;
//        }

//        if (!ExtendsContentIntoTitleBar)
//        {
//            throw new InvalidOperationException(
//                $"Cannot apply backdrop effect if {nameof(ExtendsContentIntoTitleBar)} is false."
//            );
//        }

//        if (WindowBackdrop.IsSupported(newValue) && WindowBackdrop.RemoveBackground(this))
//        {
//            _ = WindowBackdrop.ApplyBackdrop(this, newValue);

//            _ = WindowBackdrop.RemoveTitlebarBackground(this);
//        }
//    }

//    /// <summary>
//    /// Private <see cref="ExtendsContentIntoTitleBar"/> property callback.
//    /// </summary>
//    private static void OnExtendsContentIntoTitleBarChanged(
//        DependencyObject d,
//        DependencyPropertyChangedEventArgs e
//    )
//    {
//        if (d is not FluentWindow window)
//        {
//            return;
//        }

//        if (e.OldValue == e.NewValue)
//        {
//            return;
//        }

//        window.OnExtendsContentIntoTitleBarChanged((bool)e.OldValue, (bool)e.NewValue);
//    }

//    /// <summary>
//    /// This virtual method is called when <see cref="ExtendsContentIntoTitleBar"/> is changed.
//    /// </summary>
//    protected virtual void OnExtendsContentIntoTitleBarChanged(bool oldValue, bool newValue)
//    {
//        // AllowsTransparency = true;
//        SetCurrentValue(WindowStyleProperty, WindowStyle.SingleBorderWindow);

//        WindowChrome.SetWindowChrome(
//            this,
//            new WindowChrome
//            {
//                CaptionHeight = 0,
//                CornerRadius = default,
//                GlassFrameThickness = new Thickness(-1),
//                ResizeBorderThickness = ResizeMode == ResizeMode.NoResize ? default : new Thickness(4),
//                UseAeroCaptionButtons = false
//            }
//        );

//        // WindowStyleProperty.OverrideMetadata(typeof(FluentWindow), new FrameworkPropertyMetadata(WindowStyle.SingleBorderWindow));
//        // AllowsTransparencyProperty.OverrideMetadata(typeof(FluentWindow), new FrameworkPropertyMetadata(false));
//        _ = UnsafeNativeMethods.RemoveWindowTitlebarContents(this);
//    }
//}