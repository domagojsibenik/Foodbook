namespace Foodbook.DTO
{
    public class RecipeResponseDTO
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int CookingTimeInMinutes { get; set; }

        public string ImageUrl { get; set; } = string.Empty;

        public UserSummaryDTO User { get; set; } = null!;

        public List<CommentResponseDTO> Comments { get; set; } = new();

        public List<LikeResponseDTO> Likes { get; set; } = new();

        public int LikesCount => Likes.Count;

        public int CommentsCount => Comments.Count;
    }
}