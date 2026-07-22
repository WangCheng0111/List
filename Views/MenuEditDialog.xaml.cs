using System;
using List.Models;
using List.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Animation;
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
            if (sender is not Button btn || btn.CommandParameter is not Ingredient ing)
                return;

            var grid = FindParentGrid(btn);
            if (grid is not null)
            {
                FadeOutAndExecute(grid, () => ViewModel.RemoveIngredientCommand.Execute(ing));
            }
            else
            {
                ViewModel.RemoveIngredientCommand.Execute(ing);
            }
        }

        private void OnRemoveStepClick(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn || btn.CommandParameter is not RecipeStep step)
                return;

            var grid = FindParentGrid(btn);
            if (grid is not null)
            {
                FadeOutAndExecute(grid, () => ViewModel.RemoveStepCommand.Execute(step));
            }
            else
            {
                ViewModel.RemoveStepCommand.Execute(step);
            }
        }

        private static Grid? FindParentGrid(DependencyObject child)
        {
            var parent = VisualTreeHelper.GetParent(child);
            while (parent is not null)
            {
                if (parent is Grid grid && grid.ColumnDefinitions.Count > 0)
                    return grid;
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }

        private static void FadeOutAndExecute(UIElement target, Action onComplete)
        {
            var storyboard = new Storyboard();
            var fadeAnim = new DoubleAnimation
            {
                To = 0,
                Duration = new Duration(TimeSpan.FromMilliseconds(250)),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };
            Storyboard.SetTarget(fadeAnim, target);
            Storyboard.SetTargetProperty(fadeAnim, "Opacity");
            storyboard.Children.Add(fadeAnim);

            storyboard.Completed += (s, a) =>
            {
                target.Opacity = 1;
                onComplete();
            };
            storyboard.Begin();
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
