using Foodbook.DTO;
using Foodbook.Interfaces;
using Foodbook.Models;
using Foodbook.Repository;
using Foodbook.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Foodbook.Controllers
{
    [Route("api/like")]
    [ApiController]
    
    public class LikeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILikeRepository _repository;

        public LikeController(AppDbContext context, UserManager<AppUser> userManager, ILikeRepository repository)
        {
            _context = context;
            _userManager = userManager;
            _repository = repository;
        }


        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var recipes = await _repository.getAllAsync();

            return Ok(recipes);
        }

        

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var recipe = await _repository.getOneAsync(id);

            if(recipe == null)
            {
                return NotFound();
            }

            return Ok(recipe);
        }

        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] LikeDTO like)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            bool likeCheck = await _repository.doesExist(like.RecipeId, user.Id);

            if (likeCheck)
            {
                return BadRequest("Already exists");
            }

            var likeModel = await _repository.createAsync(like, user.Id);
            
            return Ok(likeModel);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            var like = await _repository.getOneAsync(id);

            if (like == null)
                return NotFound();

            if (user.Id != like.UserId)
                return Forbid();

            var removedLike = await _repository.deleteAsync(like.Id);

            return NoContent();
        }

    }
}
