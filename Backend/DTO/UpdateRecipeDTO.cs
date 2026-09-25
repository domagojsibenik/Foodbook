using System.ComponentModel.DataAnnotations;

namespace Foodbook.DTO
{
    public class UpdateRecipeDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; } = string.Empty;

        [Range(1, 1440)]
        public int CookingTimeInMinutes { get; set; }

        [Required]
        [Url]
        [MaxLength(2048)]
        public string ImageUrl { get; set; } = string.Empty;
    }
}