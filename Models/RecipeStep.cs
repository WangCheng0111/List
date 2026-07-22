using CommunityToolkit.Mvvm.ComponentModel;

namespace List.Models
{
    public partial class RecipeStep : ObservableObject
    {
        [ObservableProperty]
        public partial string Text { get; set; } = string.Empty;

        [ObservableProperty]
        public partial int Index { get; set; } = 1;
    }
}
