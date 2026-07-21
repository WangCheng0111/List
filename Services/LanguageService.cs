using System;
using Windows.Globalization;
using Windows.Storage;
using Windows.System.UserProfile;

namespace List.Services
{
    public class LanguageService
    {
        public const string SettingsKey = "AppLanguage";

        public string? CurrentLanguage { get; private set; }

        public string EffectiveLanguage => CurrentLanguage ?? GetSystemLanguage();

        public void Initialize()
        {
            var values = ApplicationData.Current.LocalSettings.Values;
            if (values.TryGetValue(SettingsKey, out var raw) && raw is string s && !string.IsNullOrEmpty(s))
                CurrentLanguage = s;

            ApplicationLanguages.PrimaryLanguageOverride = EffectiveLanguage;
        }

        public void SetLanguage(string? language)
        {
            CurrentLanguage = language;
            ApplicationData.Current.LocalSettings.Values[SettingsKey] = language ?? string.Empty;
            ApplicationLanguages.PrimaryLanguageOverride = EffectiveLanguage;
        }

        private static string GetSystemLanguage()
        {
            return GlobalizationPreferences.Languages.Count > 0
                ? GlobalizationPreferences.Languages[0]
                : "en-US";
        }
    }
}
