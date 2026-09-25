namespace Foodbook.DTO
{
    public class LikeResponseDTO
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }

        public DateTime CreatedAt { get; set; }

        public UserSummaryDTO User { get; set; } = null!;
    }
}