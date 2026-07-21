using System;
using List.Models;
using List.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace List.Views
{
    public sealed partial class MenuEditDialog : ContentDialog
    {
        private static readonly ResourceLoader Res = ResourceLoader.GetForViewIndependentUse();

        public MenuEditDialogViewModel ViewModel { get; } = new(new Services.ImageService());

        public MenuItem? Result { get; private set; }

        public MenuEditDialog()
        {
            InitializeComponent();
        }

        public void Initialize(MenuItem? item)
        {
            ViewModel.Initialize(item);
            if (item is not null)
                Title = Res.GetString("MenuEditDialog_EditTitle");
            difficultyComboBox.SelectedIndex = (int)ViewModel.Difficulty;
        }

        private void OnPrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (ViewModel.TryGetResult(out var result))
            {
                Result = result;
            }
            else
            {
                args.Cancel = true;
            }
        }

        private void OnDifficultySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (difficultyComboBox.SelectedIndex >= 0)
                ViewModel.Difficulty = (Difficulty)difficultyComboBox.SelectedIndex;
        }

        private void OnRemoveIngredientClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is Ingredient ing)
                ViewModel.RemoveIngredientCommand.Execute(ing);
        }

        private void OnRemoveStepClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is RecipeStep step)
                ViewModel.RemoveStepCommand.Execute(step);
        }

        private async void OnPickImageClick(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                FileTypeFilter = { ".png", ".jpg", ".jpeg", ".bmp", ".gif" }
            };

            var hwnd = App.MainWindow is not null
                ? WindowNative.GetWindowHandle(App.MainWindow)
                : IntPtr.Zero;
            if (hwnd != IntPtr.Zero)
                InitializeWithWindow.Initialize(picker, hwnd);

            var file = await picker.PickSingleFileAsync();
            if (file is not null)
                await ViewModel.SetImageFromPickedFileAsync(file);
        }
    }
}
