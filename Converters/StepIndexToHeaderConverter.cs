using System;
using Microsoft.UI.Xaml.Data;

namespace List.Converters
{
    public class StepIndexToHeaderConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, string language)
        {
            if (value is int index && index >= 1)
            {
                var res = Windows.ApplicationModel.Resources.ResourceLoader.GetForViewIndependentUse();
                var format = res.GetString("Step_HeaderFormat");
                return string.Format(format, index);
            }
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, string language)
            => throw new NotImplementedException();
    }
}
