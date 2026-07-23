using System;
using List.Models;
using List.Services;
using List.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Windows.ApplicationModel.Resources;

namespace List.Views
{
    public sealed partial class HomePage : Page
    {
        private static readonly ResourceLoader Res = ResourceLoader.GetForViewIndependentUse();

        public HomePageViewModel ViewModel { get; }

        public HomePage()
        {
            InitializeComponent();
            ViewModel = new HomePageViewModel(new JsonStorageService(), new ImageService());
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _ = ViewModel.LoadAsync();
        }

        private async void OnAddClick(object sender, RoutedEventArgs e)
        {
            var dialog = new MenuEditDialog();
            dialog.Initialize(null);
            dialog.XamlRoot = this.XamlRoot;
            dialog.RequestedTheme = this.ActualTheme;
            await dialog.ShowAsync();

            if (dialog.Result is not null)
            {
                ViewModel.Items.Add(dialog.Result);
                await ViewModel.SaveAsync();
            }
        }

        private async void OnItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is not MenuItem item)
                return;

            var dialog = new MenuEditDialog();
            dialog.Initialize(item);
            dialog.XamlRoot = this.XamlRoot;
            dialog.RequestedTheme = this.ActualTheme;
            await dialog.ShowAsync();

            if (dialog.Result is not null)
            {
                var index = ViewModel.Items.IndexOf(dialog.Result);
                if (index >= 0)
                    ViewModel.Items[index] = dialog.Result;
                await ViewModel.SaveAsync();
            }
        }

        private async void OnDeleteItemClick(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn || btn.CommandParameter is not MenuItem item)
                return;

            var dialog = new ContentDialog
            {
                XamlRoot = this.XamlRoot,
                RequestedTheme = this.ActualTheme,
                Title = Res.GetString("MenuDelete_Title"),
                Content = string.Format(Res.GetString("MenuDelete_Message"), item.Name),
                PrimaryButtonText = Res.GetString("MenuDelete_Delete"),
                CloseButtonText = Res.GetString("MenuDelete_Cancel"),
                DefaultButton = ContentDialogButton.Close,
                Style = (Style)Application.Current.Resources["DefaultContentDialogStyle"]
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
                await ViewModel.DeleteAsync(item);
        }
    }
}
