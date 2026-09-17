namespace Foodbook.DTO
{
    public class LikeDTO
    {
        public int RecipeId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
