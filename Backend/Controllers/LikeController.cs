using Foodbook.DTO;
using Foodbook.Models;
using Foodbook.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Foodbook.Controllers
{
    [Route("api/user")]
    [ApiController]
    
    public class LikeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public LikeController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LikeDTO like)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            

            var likeModel = new Like()
            {
                UserId = user.Id,
                RecipeId = like.RecipeId,
                CreatedAt = DateTime.Now
            };

            var likeCheck = await _context.Likes.FindAsync(likeModel);

            if(likeCheck != null)
            {
                return Ok("Already exists");
            }

            await _context.Likes.AddAsync(likeModel);
            await _context.SaveChangesAsync();

            return Ok(likeModel);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            var like = await _context.Likes.FirstOrDefaultAsync(x => x.Id == id);
            


            if (user.Id != like.UserId)
            {
                return Unauthorized();
            }
            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
