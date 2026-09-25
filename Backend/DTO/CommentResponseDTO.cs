namespace Foodbook.DTO
{
    public class CommentResponseDTO
    {
        public int Id { get; set; }

        public string Text { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public int RecipeId { get; set; }

        public UserSummaryDTO User { get; set; } = null!;
    }
}