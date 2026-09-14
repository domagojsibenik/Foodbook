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
    [Route("[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IRecipeRepository _repository;

        public RecipeController(AppDbContext context, UserManager<AppUser> userManager, IRecipeRepository repository)
        {
            _userManager = userManager;
            _repository = repository;
        }

        [HttpGet()]
        public async Task<IActionResult> Get([FromQuery] QueryObject query)
        {
            var recipes = await _repository.GetAllAsync(query);

            return Ok(recipes);
           
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var recipe = await _repository.GetByIdAsync(id);

            RecipeDTO recipeDTO = new RecipeDTO()
            {
                Name = recipe.Name,
                Description = recipe.Description,
                CookingTimeInMinutes = recipe.CookingTimeInMinutes,
            };
            return Ok(recipe);

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RecipeDTO recipeDTO)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return Unauthorized();

            var recipe = new Recipe()
            {
                Name = recipeDTO.Name,
                Description = recipeDTO.Description,
                CookingTimeInMinutes = recipeDTO.CookingTimeInMinutes,
                UserId = user.Id
            };
            await _repository.CreateAsync(recipe);

            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task< IActionResult> Update(int id, RecipeDTO recipeDTO)
        {
            var recipe = await _repository.UpdateAsync(id, recipeDTO);

            if(recipe == null)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var recipe = await _repository.DeleteAsync(id);

            if(recipe == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
