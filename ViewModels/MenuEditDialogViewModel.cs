using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using List.Models;
using List.Services;

namespace List.ViewModels
{
    public partial class MenuEditDialogViewModel : ObservableObject
    {
        private readonly ImageService _imageService;
        private MenuItem? _editing;
        private string? _originalImagePath;

        [ObservableProperty]
        public partial string Name { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string? ImageRelativePath { get; set; }

        [ObservableProperty]
        public partial string TagsText { get; set; } = string.Empty;

        [ObservableProperty]
        public partial double CookingTimeMinutes { get; set; }

        [ObservableProperty]
        public partial Difficulty Difficulty { get; set; } = Difficulty.Easy;

        [ObservableProperty]
        public partial string? Notes { get; set; }

        [ObservableProperty]
        public partial string ErrorMessage { get; set; } = string.Empty;

        [ObservableProperty]
        public partial bool HasError { get; set; }

        partial void OnErrorMessageChanged(string value)
        {
            HasError = !string.IsNullOrEmpty(value);
        }

        public ObservableCollection<Ingredient> Ingredients { get; } = new();

        public ObservableCollection<RecipeStep> Steps { get; } = new();

        public Difficulty[] DifficultyOptions { get; } =
            (Difficulty[])Enum.GetValues(typeof(Difficulty));

        public bool IsEditMode => _editing is not null;

        public MenuEditDialogViewModel(ImageService imageService)
        {
            _imageService = imageService;
        }

        public void Initialize(MenuItem? item)
        {
            _editing = item;
            Ingredients.Clear();
            Steps.Clear();

            if (item is null)
            {
                Name = string.Empty;
                ImageRelativePath = null;
                _originalImagePath = null;
                TagsText = string.Empty;
                CookingTimeMinutes = 0;
                Difficulty = Difficulty.Easy;
                Notes = null;
                ErrorMessage = string.Empty;
                return;
            }

            Name = item.Name;
            ImageRelativePath = item.ImageRelativePath;
            _originalImagePath = item.ImageRelativePath;
            CookingTimeMinutes = item.CookingTimeMinutes ?? 0;
            Difficulty = item.Difficulty;
            Notes = item.Notes;
            TagsText = string.Join(", ", item.Tags);
            foreach (var ing in item.Ingredients)
                Ingredients.Add(new Ingredient { Name = ing.Name, Amount = ing.Amount, Unit = ing.Unit });
            foreach (var step in item.Steps)
                Steps.Add(new RecipeStep { Text = step.Text });
            RenumberSteps();
            ErrorMessage = string.Empty;
        }

        public async System.Threading.Tasks.Task SetImageFromPickedFileAsync(Windows.Storage.StorageFile file)
        {
            if (file is null)
                return;
            var rel = await _imageService.CopyImageAsync(file);
            ImageRelativePath = rel;
        }

        public bool TryGetResult(out MenuItem result)
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Name))
            {
                ErrorMessage = "菜名不能为空";
                result = null!;
                return false;
            }

            var tags = TagsText
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .ToList();

            if (_editing is null)
            {
                result = new MenuItem
                {
                    Name = Name.Trim(),
                    ImageRelativePath = ImageRelativePath,
                    Ingredients = Ingredients.Select(i => new Ingredient { Name = i.Name, Amount = i.Amount, Unit = i.Unit }).ToList(),
                    Steps = Steps.Select(s => new RecipeStep { Text = s.Text }).ToList(),
                    Tags = tags,
                    CookingTimeMinutes = CookingTimeMinutes > 0 ? (int)Math.Round(CookingTimeMinutes) : null,
                    Difficulty = Difficulty,
                    Notes = Notes,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
            }
            else
            {
                _editing.Name = Name.Trim();
                _editing.ImageRelativePath = ImageRelativePath;
                _editing.Ingredients = Ingredients.Select(i => new Ingredient { Name = i.Name, Amount = i.Amount, Unit = i.Unit }).ToList();
                _editing.Steps = Steps.Select(s => new RecipeStep { Text = s.Text }).ToList();
                _editing.Tags = tags;
                _editing.CookingTimeMinutes = CookingTimeMinutes > 0 ? (int)Math.Round(CookingTimeMinutes) : null;
                _editing.Difficulty = Difficulty;
                _editing.Notes = Notes;
                _editing.UpdatedAt = DateTime.Now;
                result = _editing;

                if (_originalImagePath != _editing.ImageRelativePath)
                    _ = _imageService.DeleteImageAsync(_originalImagePath);
            }

            return true;
        }

        private void RenumberSteps()
        {
            for (int i = 0; i < Steps.Count; i++)
                Steps[i].Index = i + 1;
        }

        [RelayCommand]
        private void AddIngredient() => Ingredients.Add(new Ingredient());

        [RelayCommand]
        private void RemoveIngredient(Ingredient? ingredient)
        {
            if (ingredient is not null)
                Ingredients.Remove(ingredient);
        }

        [RelayCommand]
        private void AddStep()
        {
            Steps.Add(new RecipeStep());
            RenumberSteps();
        }

        [RelayCommand]
        private void RemoveStep(RecipeStep? step)
        {
            if (step is not null)
            {
                Steps.Remove(step);
                RenumberSteps();
            }
        }
    }
}
