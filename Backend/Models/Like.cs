namespace Foodbook.Models
{
    public class Like
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int RecipeId { get; set; }
        public DateTime CreatedAt { get; set; }

        public Recipe Recipe { get; set; }
        public AppUser AppUser { get; set; }
    }
}
