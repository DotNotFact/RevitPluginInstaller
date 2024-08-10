//using RevitPluginInstaller.Resources.CustomWindow;
//using RevitPluginInstaller.Managers.Bases;
//using System.Runtime.InteropServices;
//using System.Windows.Interop;
//using System.Windows.Shell;
//using System.Windows;

//namespace RevitPluginInstaller.Resources.FluentWindow;

//public class FluentWindow : Window
//{
//    private WindowInteropHelper? _interopHelper = null;
 
//    protected WindowInteropHelper InteropHelper => _interopHelper ??= new WindowInteropHelper(this);

//    public static readonly DependencyProperty CornerPreferenceProperty = DependencyProperty.Register(
//        nameof(CornerPreference),
//        typeof(CornerPreference), 
//        typeof(FluentWindow),
//        new PropertyMetadata(CornerPreference.Round, OnWindowCornerPreferenceChanged)
//    );

//    public CornerPreference CornerPreference
//    {
//        get => (CornerPreference)GetValue(CornerPreferenceProperty);
//        set => SetValue(CornerPreferenceProperty, value);
//    }

//    public static readonly DependencyProperty BackdropTypeProperty = DependencyProperty.Register(
//        nameof(BackdropType),
//        typeof(BackdropType),
//        typeof(FluentWindow),
//        new PropertyMetadata(BackdropType.Mica, OnWindowBackdropTypeChanged)
//    );
     
//    public static readonly DependencyProperty ExtendsContentIntoTitleBarProperty = DependencyProperty.Register(
//        nameof(ExtendsContentIntoTitleBar),
//        typeof(bool),
//        typeof(FluentWindow),
//        new PropertyMetadata(false, OnExtendsContentIntoTitleBarChanged)
//    ); 

//    public BackdropType BackdropType
//    {
//        get => (BackdropType)GetValue(BackdropTypeProperty);
//        set => SetValue(BackdropTypeProperty, value);
//    }

//    public bool ExtendsContentIntoTitleBar
//    {
//        get => (bool)GetValue(ExtendsContentIntoTitleBarProperty);
//        set => SetValue(ExtendsContentIntoTitleBarProperty, value);
//    }

//    static FluentWindow()
//    {
//        // DefaultStyleKeyProperty.OverrideMetadata(typeof(FluentWindow), new FrameworkPropertyMetadata(typeof(FluentWindow)));
//    }

//    public FluentWindow()
//    {
//        // DefaultStyleKey = typeof(FluentWindow);

//        ApplyWindowCornerPreference();
//        ApplyWindowBackdropType();
//        ApplyExtendsContentIntoTitleBar(); 
//    }

//    protected override void OnSourceInitialized(EventArgs e)
//    {
//        base.OnSourceInitialized(e);
//    }

//    private static void OnWindowCornerPreferenceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
//    {
//        if (d is FluentWindow window)
//        {
//            window.ApplyWindowCornerPreference();
//        }
//    }

//    private static void OnWindowBackdropTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
//    {
//        if (d is FluentWindow window)
//        {
//            window.ApplyWindowBackdropType();
//        }
//    }

//    private static void OnExtendsContentIntoTitleBarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
//    {
//        if (d is FluentWindow window)
//        {
//            window.ApplyExtendsContentIntoTitleBar();
//        }
//    }

//    private void ApplyWindowCornerPreference()
//    {
//        if (InteropHelper.Handle == nint.Zero) return;

//        if (IsWindows11OrGreater())
//        {
//            var attribute = DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
//            var preference = (int)CornerPreference;

//            DwmSetWindowAttribute(InteropHelper.Handle, attribute, ref preference, sizeof(int));
//        }
//    }

//    private void ApplyWindowBackdropType()
//    {
//        if (InteropHelper.Handle == nint.Zero) return;

//        if (IsWindows11OrGreater())
//        {
//            var attribute = DWMWINDOWATTRIBUTE.DWMWA_SYSTEMBACKDROP_TYPE;
//            var backdropType = (int)BackdropType;
//            DwmSetWindowAttribute(InteropHelper.Handle, attribute, ref backdropType, sizeof(int));

//            if (BackdropType != BackdropType.None)
//            {
//                EnableBlur();
//            }
//            else
//            {
//                DisableBlur();
//            }
//        }
//        else if (IsWindows10OrGreater())
//        {
//            // Для Windows 10 можно применить эффект Acrylic
//            if (BackdropType == BackdropType.Acrylic)
//            {
//                EnableBlur();
//            }
//            else
//            {
//                DisableBlur();
//            }
//        }
//    }

//    private void ApplyExtendsContentIntoTitleBar()
//    {
//        if (ExtendsContentIntoTitleBar)
//        {
//            WindowStyle = WindowStyle.None;
//            AllowsTransparency = true;

//            WindowChrome.SetWindowChrome(this, new WindowChrome
//            {
//                CaptionHeight = 0,
//                ResizeBorderThickness = new Thickness(5),
//                CornerRadius = new CornerRadius(0),
//                GlassFrameThickness = new Thickness(-1)
//            });

//            // Удаление стандартной рамки окна
//            if (InteropHelper.Handle != nint.Zero)
//            {
//                int style = GetWindowLong(InteropHelper.Handle, -16);
//                style &= ~(WS_CAPTION | WS_THICKFRAME);
//                SetWindowLong(InteropHelper.Handle, -16, style);
//            }
//        }
//    }

//    private void EnableBlur()
//    {
//        var windowHelper = new WindowInteropHelper(this);

//        var accent = new AccentPolicy
//        {
//            AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND
//        };

//        var accentStructSize = Marshal.SizeOf(accent);
//        var accentPtr = Marshal.AllocHGlobal(accentStructSize);
//        Marshal.StructureToPtr(accent, accentPtr, false);

//        var data = new WindowCompositionAttributeData
//        {
//            Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
//            SizeOfData = accentStructSize,
//            Data = accentPtr
//        };

//        SetWindowCompositionAttribute(windowHelper.Handle, ref data);

//        Marshal.FreeHGlobal(accentPtr);
//    }

//    private void DisableBlur()
//    {
//        var windowHelper = new WindowInteropHelper(this);

//        var accent = new AccentPolicy
//        {
//            AccentState = AccentState.ACCENT_DISABLED
//        };

//        var accentStructSize = Marshal.SizeOf(accent);
//        var accentPtr = Marshal.AllocHGlobal(accentStructSize);
//        Marshal.StructureToPtr(accent, accentPtr, false);

//        var data = new WindowCompositionAttributeData
//        {
//            Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
//            SizeOfData = accentStructSize,
//            Data = accentPtr
//        };

//        SetWindowCompositionAttribute(windowHelper.Handle, ref data);

//        Marshal.FreeHGlobal(accentPtr);
//    }

//    private bool IsWindows10OrGreater()
//    {
//        return Environment.OSVersion.Version.Major >= 10;
//    }

//    private bool IsWindows11OrGreater()
//    {
//        return Environment.OSVersion.Version.Major >= 10 && Environment.OSVersion.Version.Build >= 22000;
//    }

//    // Метод для применения темы
//    public void ApplyTheme(Theme theme)
//    {
//        if (PresentationSource.FromVisual(this) is HwndSource hwndSource)
//        {
//            switch (theme)
//            {
//                case Theme.Dark:
//                    WindowsApiInterop.SetDarkMode(hwndSource.Handle, true);
//                    // Применить темную тему к элементам управления
//                    break;
//                case Theme.Light:
//                    WindowsApiInterop.SetDarkMode(hwndSource.Handle, false);
//                    // Применить светлую тему к элементам управления
//                    break;
//                case Theme.System:
//                    // Определить системную тему и применить соответствующие настройки
//                    break;
//            }

//            // Обновляем ресурсы окна
//            var newDict = new ResourceDictionary
//            {
//                Source = new Uri($"pack://application:,,,/RevitPluginInstaller;component/Resources/Themes/{theme}.xaml", UriKind.Absolute)
//            };

//            Resources.MergedDictionaries.Clear();
//            Resources.MergedDictionaries.Add(newDict);
//        }
//    }

//    // Windows API
//    [DllImport("dwmapi.dll")]
//    private static extern int DwmSetWindowAttribute(nint hwnd, DWMWINDOWATTRIBUTE attribute, ref int pvAttribute, int cbAttribute);

//    [DllImport("user32.dll")]
//    private static extern int GetWindowLong(nint hWnd, int nIndex);

//    [DllImport("user32.dll")]
//    private static extern int SetWindowLong(nint hWnd, int nIndex, int dwNewLong);

//    [DllImport("user32.dll")]
//    private static extern bool SetWindowCompositionAttribute(nint hwnd, ref WindowCompositionAttributeData data);

//    [StructLayout(LayoutKind.Sequential)]
//    private struct AccentPolicy
//    {
//        public AccentState AccentState;
//        public int AccentFlags;
//        public int GradientColor;
//        public int AnimationId;
//    }

//    [StructLayout(LayoutKind.Sequential)]
//    private struct WindowCompositionAttributeData
//    {
//        public WindowCompositionAttribute Attribute;
//        public nint Data;
//        public int SizeOfData;
//    }

//    private enum AccentState
//    {
//        ACCENT_DISABLED = 0,
//        ACCENT_ENABLE_GRADIENT = 1,
//        ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
//        ACCENT_ENABLE_BLURBEHIND = 3,
//        ACCENT_ENABLE_ACRYLICBLURBEHIND = 4,
//        ACCENT_INVALID_STATE = 5
//    }

//    private enum WindowCompositionAttribute
//    {
//        WCA_ACCENT_POLICY = 19
//    }

//    private enum DWMWINDOWATTRIBUTE
//    {
//        DWMWA_WINDOW_CORNER_PREFERENCE = 33,
//        DWMWA_SYSTEMBACKDROP_TYPE = 38
//    }

//    private const int WS_CAPTION = 0x00C00000;
//    private const int WS_THICKFRAME = 0x00040000;
//}


//public enum CornerPreference
//{
//    Default,
//    Round,
//    Small,
//    None
//}

//public enum BackdropType
//{
//    Default,
//    None,
//    Mica,
//    Acrylic
//}