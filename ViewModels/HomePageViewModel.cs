using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using List.Models;
using List.Services;

namespace List.ViewModels
{
    public partial class HomePageViewModel : ObservableObject
    {
        private readonly JsonStorageService _storage;
        private readonly ImageService _images;

        public ObservableCollection<MenuItem> Items { get; } = new();

        [ObservableProperty]
        public partial bool IsEmpty { get; set; } = true;

        public HomePageViewModel(JsonStorageService storage, ImageService images)
        {
            _storage = storage;
            _images = images;
            Items.CollectionChanged += OnItemsChanged;
        }

        private void OnItemsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            IsEmpty = Items.Count == 0;
        }

        [RelayCommand]
        public async Task LoadAsync()
        {
            Items.Clear();
            var list = await _storage.LoadAsync();
            foreach (var item in list)
                Items.Add(item);
        }

        [RelayCommand]
        public async Task SaveAsync()
        {
            await _storage.SaveAsync(Items);
        }

        public async Task DeleteAsync(MenuItem item)
        {
            Items.Remove(item);
            await _images.DeleteImageAsync(item.ImageRelativePath);
            await _storage.SaveAsync(Items);
        }
    }
}
