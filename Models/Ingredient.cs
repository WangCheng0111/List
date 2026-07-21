using CommunityToolkit.Mvvm.ComponentModel;

namespace List.Models
{
    public partial class Ingredient : ObservableObject
    {
        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Amount { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string Unit { get; set; } = string.Empty;
    }
}
