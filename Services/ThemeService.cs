using System;
using Microsoft.UI.Xaml;
using Windows.Storage;

namespace List.Services
{
    public class ThemeService
    {
        public const string SettingsKey = "AppTheme";

        public ElementTheme CurrentTheme { get; private set; } = ElementTheme.Default;

        public event Action<ElementTheme>? ThemeChanged;

        public void Initialize()
        {
            var values = ApplicationData.Current.LocalSettings.Values;
            if (values.TryGetValue(SettingsKey, out var raw) && raw is string s)
            {
                if (Enum.TryParse(s, out ElementTheme theme))
                    CurrentTheme = theme;
            }
        }

        public void SetTheme(ElementTheme theme)
        {
            if (theme == CurrentTheme)
                return;

            CurrentTheme = theme;
            ApplicationData.Current.LocalSettings.Values[SettingsKey] = theme.ToString();
            ThemeChanged?.Invoke(theme);
        }

        public void ToggleTheme()
        {
            SetTheme(CurrentTheme == ElementTheme.Dark ? ElementTheme.Light : ElementTheme.Dark);
        }
    }
}
