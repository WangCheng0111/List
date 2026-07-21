using List.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace List.Views
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPageViewModel ViewModel { get; }

        private bool _initialized;

        public SettingsPage()
        {
            InitializeComponent();
            ViewModel = new SettingsPageViewModel(App.ThemeService);
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // Light=0, Dark=1, Default=2 (same order as WinUI Gallery)
            themeMode.SelectedIndex = ViewModel.CurrentTheme switch
            {
                ElementTheme.Light => 0,
                ElementTheme.Dark => 1,
                _ => 2
            };

            // zh-CN=0, en-US=1
            languageMode.SelectedIndex = App.LanguageService.EffectiveLanguage.StartsWith("zh") ? 0 : 1;

            _initialized = true;
        }

        private void OnThemeSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_initialized)
                return;

            if ((themeMode.SelectedItem as ComboBoxItem)?.Tag?.ToString() is not string selectedTheme)
                return;

            switch (selectedTheme)
            {
                case "Light":
                    ViewModel.SetLightCommand.Execute(null);
                    break;
                case "Dark":
                    ViewModel.SetDarkCommand.Execute(null);
                    break;
                default:
                    ViewModel.UseSystemCommand.Execute(null);
                    break;
            }
        }

        private void OnLanguageSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_initialized)
                return;

            if ((languageMode.SelectedItem as ComboBoxItem)?.Tag?.ToString() is not string selectedLanguage)
                return;

            App.LanguageService.SetLanguage(selectedLanguage);
            languageRestartBar.IsOpen = true;
        }
    }
}
