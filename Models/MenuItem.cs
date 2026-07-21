using System;
using System.Collections.Generic;

namespace List.Models
{
    public class MenuItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string? ImageRelativePath { get; set; }
        public List<Ingredient> Ingredients { get; set; } = new();
        public List<RecipeStep> Steps { get; set; } = new();
        public List<string> Tags { get; set; } = new();
        public int? CookingTimeMinutes { get; set; }
        public Difficulty Difficulty { get; set; } = Difficulty.Easy;
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
