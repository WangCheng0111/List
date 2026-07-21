using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using List.Models;
using Windows.Storage;

namespace List.Services
{
    public class JsonStorageService
    {
        private const string FileName = "menuitems.json";

        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters = { new JsonStringEnumConverter() }
        };

        public async Task<List<MenuItem>> LoadAsync()
        {
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.TryGetItemAsync(FileName) as StorageFile;
            if (file is null)
                return new List<MenuItem>();

            var json = await FileIO.ReadTextAsync(file);
            if (string.IsNullOrWhiteSpace(json))
                return new List<MenuItem>();

            return JsonSerializer.Deserialize<List<MenuItem>>(json, Options) ?? new List<MenuItem>();
        }

        public async Task SaveAsync(IEnumerable<MenuItem> items)
        {
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            var json = JsonSerializer.Serialize(items, Options);
            await FileIO.WriteTextAsync(file, json);
        }
    }
}
