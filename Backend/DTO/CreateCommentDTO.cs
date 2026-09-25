using System.ComponentModel.DataAnnotations;

namespace Foodbook.DTO
{
    public class CreateCommentDTO
    {
        [Required]
        [MaxLength(1000)]
        public string Text { get; set; } = string.Empty;

        public int RecipeId { get; set; }
    }
}