using System;
using Microsoft.UI.Xaml.Data;

namespace List.Converters
{
    public class CookingTimeToStringConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, string language)
        {
            if (value is int minutes)
                return $"{minutes} 分钟";
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, string language)
            => throw new NotImplementedException();
    }
}
