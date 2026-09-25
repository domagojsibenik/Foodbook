using Microsoft.AspNetCore.Identity;

namespace Foodbook.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Like> Likes { get; set; }= new List<Like>();
    }
}
