using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace List.Services
{
    public class ImageService
    {
        private const string ImageFolderName = "images";

        public async Task<string?> CopyImageAsync(StorageFile source)
        {
            if (source is null)
                return null;

            var localFolder = ApplicationData.Current.LocalFolder;
            var imageFolder = await localFolder.CreateFolderAsync(ImageFolderName, CreationCollisionOption.OpenIfExists);
            var copied = await source.CopyAsync(imageFolder, source.Name, NameCollisionOption.GenerateUniqueName);
            return $"{ImageFolderName}/{copied.Name}";
        }

        public string GetFullPath(string? relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return string.Empty;

            return Path.Combine(ApplicationData.Current.LocalFolder.Path, relativePath);
        }

        public async Task DeleteImageAsync(string? relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return;

            try
            {
                var file = await ApplicationData.Current.LocalFolder.TryGetItemAsync(relativePath) as StorageFile;
                if (file is not null)
                    await file.DeleteAsync();
            }
            catch
            {
            }
        }
    }
}
