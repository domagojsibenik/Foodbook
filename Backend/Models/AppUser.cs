using Microsoft.AspNetCore.Identity;

namespace Foodbook.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
    }
}
