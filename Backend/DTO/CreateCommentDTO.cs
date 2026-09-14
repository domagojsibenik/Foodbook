using Foodbook.Models;

namespace Foodbook.DTO
{
    public class CreateCommentDTO
    {
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? RecipeId { get; set; }
    }
}
