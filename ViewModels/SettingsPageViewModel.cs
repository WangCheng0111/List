using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using List.Services;
using Microsoft.UI.Xaml;

namespace List.ViewModels
{
    public partial class SettingsPageViewModel : ObservableObject
    {
        private readonly ThemeService _themeService;

        [ObservableProperty]
        public partial ElementTheme CurrentTheme { get; set; }

        public SettingsPageViewModel(ThemeService themeService)
        {
            _themeService = themeService;
            CurrentTheme = themeService.CurrentTheme;
            _themeService.ThemeChanged += t => CurrentTheme = t;
        }

        [RelayCommand]
        public void SetLight() => _themeService.SetTheme(ElementTheme.Light);

        [RelayCommand]
        public void SetDark() => _themeService.SetTheme(ElementTheme.Dark);

        [RelayCommand]
        public void UseSystem() => _themeService.SetTheme(ElementTheme.Default);
    }
}
