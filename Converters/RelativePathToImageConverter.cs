using System;
using List.Services;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Storage;

namespace List.Converters
{
    public class RelativePathToImageConverter : IValueConverter
    {
        private static readonly ImageService ImageService = new();

        public object? Convert(object? value, Type targetType, object? parameter, string language)
        {
            if (value is not string relativePath || string.IsNullOrEmpty(relativePath))
                return null;

            var fullPath = ImageService.GetFullPath(relativePath);
            try
            {
                var bitmap = new BitmapImage();
                bitmap.UriSource = new Uri($"file:///{fullPath.Replace('\\', '/')}");
                return bitmap;
            }
            catch
            {
                return null;
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, string language)
            => throw new NotImplementedException();
    }
}
