using List.Services;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Globalization;
using Windows.System.UserProfile;

// To learn more about WinUI, the WinUI project structure,
// and more about the project templates, see: http://aka.ms/winui-project-info.

namespace List
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private Window? _window;
        private ThemeService? _themeService;
        private LanguageService? _languageService;

        public static MainWindow? MainWindow { get; private set; }

        public static ThemeService ThemeService
        {
            get
            {
                if (Current is App app && app._themeService is not null)
                    return app._themeService;
                throw new InvalidOperationException("ThemeService not initialized");
            }
        }

        public static LanguageService LanguageService
        {
            get
            {
                if (Current is App app && app._languageService is not null)
                    return app._languageService;
                throw new InvalidOperationException("LanguageService not initialized");
            }
        }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            _languageService = new LanguageService();
            _languageService.Initialize();

            InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _themeService = new ThemeService();
            _themeService.Initialize();
            var main = new MainWindow(_themeService);
            MainWindow = main;
            _window = main;
            _window.Activate();
        }
    }
}
