using Foodbook.DTO;
using Foodbook.Helpers;
using Foodbook.Interfaces;
using Foodbook.Models;
using Foodbook.Repository;
using Foodbook.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Foodbook.Controllers
{
    [Route("api/like")]
    [ApiController]
    
    public class LikeController : Controller
    {
    
        private readonly UserManager<AppUser> _userManager;
        private readonly ILikeRepository _repository;
        private readonly IRecipeRepository _recipeRepository;

        public LikeController(UserManager<AppUser> userManager, ILikeRepository repository, IRecipeRepository recipeRepository)
        {
            _userManager = userManager;
            _repository = repository;
            _recipeRepository = recipeRepository;
        }


        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var likes = await _repository.GetAllAsync();

            return Ok(likes.Select(l => l.ToResponseDTO()));
        }

        

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var like = await _repository.GetByIdAsync(id);

            if(like == null)
            {
                return NotFound();
            }

            return Ok(like.ToResponseDTO());
        }
        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] CreateLikeDTO likeDto)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            if (!await _recipeRepository.ExistsAsync(likeDto.RecipeId))
            {
                return NotFound("Recipe not found");
            }

            var recipe = await _recipeRepository.GetByIdAsync(likeDto.RecipeId);

            bool likeCheck = await _repository.ExistsAsync(likeDto.RecipeId, user.Id);

            if (likeCheck)
            {
                return Conflict("You already liked this recipe.");
            }
            var like = new Like
            {
                RecipeId = likeDto.RecipeId,
                UserId = user.Id,
                User = user,

                CreatedAt = DateTime.UtcNow
            };

            var likeModel = await _repository.CreateAsync(like);
            
            return Ok(likeModel.ToResponseDTO());
        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            var like = await _repository.GetByIdAsync(id);

            if (like == null)
                return NotFound();

            if (user.Id != like.UserId)
                return Forbid();

            var removedLike = await _repository.DeleteAsync(like.Id);

            return NoContent();
        }

    }
}
