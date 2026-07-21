using System;
using List.Models;
using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace List.Converters
{
    public class DifficultyToBrushConverter : IValueConverter
    {
        private static readonly SolidColorBrush EasyBrush = new(Colors.MediumSeaGreen);
        private static readonly SolidColorBrush MediumBrush = new(Colors.Orange);
        private static readonly SolidColorBrush HardBrush = new(Colors.IndianRed);
        private static readonly SolidColorBrush FallbackBrush = new(Colors.Gray);

        public object Convert(object? value, Type targetType, object? parameter, string language)
        {
            return value is Difficulty d
                ? d switch
                {
                    Difficulty.Easy => EasyBrush,
                    Difficulty.Medium => MediumBrush,
                    Difficulty.Hard => HardBrush,
                    _ => FallbackBrush
                }
                : FallbackBrush;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, string language)
            => throw new NotImplementedException();
    }
}
