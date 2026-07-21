using System;
using System.Linq;
using List.Services;
using List.Views;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Graphics;

namespace List
{
    public sealed partial class MainWindow : Window
    {
        private readonly ThemeService _themeService;

        public MainWindow(ThemeService themeService)
        {
            InitializeComponent();
            _themeService = themeService;

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
            AppWindow.TitleBar.PreferredHeightOption = TitleBarHeightOption.Tall;
            MoveAndCenterWindowOnScreen(new SizeInt32(1600, 900));

            ApplyTheme(_themeService.CurrentTheme);
            _themeService.ThemeChanged += ApplyTheme;

            // Default to Home page
            navView.SelectedItem = navView.MenuItems.OfType<NavigationViewItem>().FirstOrDefault();
        }

        private void ApplyTheme(ElementTheme theme)
        {
            rootGrid.RequestedTheme = theme;

            // 同步标题栏按钮颜色以匹配主题
            var titleBar = AppWindow.TitleBar;
            if (theme == ElementTheme.Dark)
            {
                titleBar.ButtonForegroundColor = Windows.UI.Color.FromArgb(255, 255, 255, 255);
                titleBar.ButtonBackgroundColor = Windows.UI.Color.FromArgb(0, 0, 0, 0);
                titleBar.ButtonHoverForegroundColor = Windows.UI.Color.FromArgb(255, 255, 255, 255);
                titleBar.ButtonHoverBackgroundColor = Windows.UI.Color.FromArgb(20, 255, 255, 255);
                titleBar.ButtonPressedForegroundColor = Windows.UI.Color.FromArgb(255, 255, 255, 255);
                titleBar.ButtonPressedBackgroundColor = Windows.UI.Color.FromArgb(40, 255, 255, 255);
                titleBar.ButtonInactiveForegroundColor = Windows.UI.Color.FromArgb(120, 255, 255, 255);
                titleBar.ButtonInactiveBackgroundColor = Windows.UI.Color.FromArgb(0, 0, 0, 0);
            }
            else
            {
                titleBar.ButtonForegroundColor = Windows.UI.Color.FromArgb(255, 0, 0, 0);
                titleBar.ButtonBackgroundColor = Windows.UI.Color.FromArgb(0, 0, 0, 0);
                titleBar.ButtonHoverForegroundColor = Windows.UI.Color.FromArgb(255, 0, 0, 0);
                titleBar.ButtonHoverBackgroundColor = Windows.UI.Color.FromArgb(20, 0, 0, 0);
                titleBar.ButtonPressedForegroundColor = Windows.UI.Color.FromArgb(255, 0, 0, 0);
                titleBar.ButtonPressedBackgroundColor = Windows.UI.Color.FromArgb(40, 0, 0, 0);
                titleBar.ButtonInactiveForegroundColor = Windows.UI.Color.FromArgb(120, 0, 0, 0);
                titleBar.ButtonInactiveBackgroundColor = Windows.UI.Color.FromArgb(0, 0, 0, 0);
            }
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            Type? pageType = null;
            if (args.IsSettingsSelected)
                pageType = typeof(SettingsPage);
            else if ((args.SelectedItemContainer?.Tag as string) == "Home")
                pageType = typeof(HomePage);

            if (pageType is not null && contentFrame.CurrentSourcePageType != pageType)
                contentFrame.Navigate(pageType);
        }

        private void MoveAndCenterWindowOnScreen(SizeInt32 size)
        {
            var displayArea = DisplayArea.GetFromWindowId(AppWindow.Id, DisplayAreaFallback.Primary);
            var workArea = displayArea.WorkArea;

            var x = workArea.X + Math.Max(0, (workArea.Width - size.Width) / 2);
            var y = workArea.Y + Math.Max(0, (workArea.Height - size.Height) / 2);

            AppWindow.MoveAndResize(new RectInt32(x, y, size.Width, size.Height));
        }
    }
}
