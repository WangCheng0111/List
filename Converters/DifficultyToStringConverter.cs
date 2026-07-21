using System;
using List.Models;
using Microsoft.UI.Xaml.Data;

namespace List.Converters
{
    public class DifficultyToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, string language)
        {
            if (value is Difficulty d)
            {
                return d switch
                {
                    Difficulty.Easy => "简单",
                    Difficulty.Medium => "中等",
                    Difficulty.Hard => "困难",
                    _ => d.ToString()
                };
            }
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, string language)
            => throw new NotImplementedException();
    }
}
