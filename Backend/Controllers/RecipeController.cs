using Foodbook.DTO;
using Foodbook.Helpers;
using Foodbook.Interfaces;
using Foodbook.Models;
using Foodbook.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Foodbook.Controllers
{
    [ApiController]
    [Route("api/recipe")]
    public class RecipeController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IRecipeRepository _repository;

        public RecipeController(UserManager<AppUser> userManager, IRecipeRepository repository)
        {
            _userManager = userManager;
            _repository = repository;
        }

        [HttpGet()]
        public async Task<IActionResult> Get([FromQuery] QueryObject query)
        {
            var recipes = await _repository.GetAllAsync(query);

            return Ok(recipes.Select(r => r.ToResponseDTO()));
           
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var recipe = await _repository.GetByIdAsync(id);

            if (recipe == null)
            {
                return NotFound();
            }
            return Ok(recipe.ToResponseDTO());

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRecipeDTO recipeDTO)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            var recipe = new Recipe()
            {
                Name = recipeDTO.Name,
                Description = recipeDTO.Description,
                CookingTimeInMinutes = recipeDTO.CookingTimeInMinutes,
                ImageUrl = recipeDTO.ImageUrl,
                UserId = user.Id,
                User = user
            };
            await _repository.CreateAsync(recipe);

            var response = new RecipeResponseDTO
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                CookingTimeInMinutes =
                    recipe.CookingTimeInMinutes,

                ImageUrl = recipe.ImageUrl,

                User = user.ToSummaryDTO()
            };

            return CreatedAtAction(nameof(GetOne),new { id = recipe.Id },response);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task< IActionResult> Update(int id, [FromBody] UpdateRecipeDTO recipeDTO)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var recipe = await _repository.GetByIdAsync(id);

            if(recipe == null)
            {
                return NotFound();
            }
            if (recipe.UserId != user.Id)
            {
                return Forbid();
            }

            recipe.Name = recipeDTO.Name;
            recipe.Description = recipeDTO.Description;
            recipe.CookingTimeInMinutes = recipeDTO.CookingTimeInMinutes;
            recipe.ImageUrl = recipeDTO.ImageUrl;

            await _repository.UpdateAsync(recipe);

            return NoContent();

        }

        [Authorize]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            var recipe = await _repository.GetByIdAsync(id);

            if(recipe == null)
            {
                return NotFound();
            }

            if (recipe.UserId != user.Id)
            {
                return Forbid();
            }

            await _repository.DeleteAsync(recipe.Id);

            return NoContent();
        }
    }
}
