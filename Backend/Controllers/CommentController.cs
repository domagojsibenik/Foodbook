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
    [Route("api/comment")]
    [ApiController]
    public class CommentController : Controller
    {

        private readonly UserManager<AppUser> _userManager;
        private readonly ICommentRepository _repository;
        private readonly IRecipeRepository _recipeRepository;


        public CommentController(UserManager<AppUser> userManager, ICommentRepository repository, IRecipeRepository recipeRepository)
        {
           
            _userManager = userManager;
            _repository = repository;
            _recipeRepository = recipeRepository;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var comment = await _repository.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment.ToResponseDTO());
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _repository.GetAllAsync();
            return Ok(comments.Select(c => c.ToResponseDTO()));
        }

        [Authorize]
        [HttpPost()]
        public async Task<IActionResult> Create([FromBody] CreateCommentDTO commentDTO)
        {
            var user = await _userManager.GetUserAsync(User);

            if(user == null)
            {
                return Unauthorized();
            }

            if (!await _recipeRepository.ExistsAsync(commentDTO.RecipeId))
            {
                return NotFound("Recipe does not exist.");
            }

            var comment = new Comment()
            {
                Text = commentDTO.Text,
                RecipeId = commentDTO.RecipeId,
                CreatedAt = DateTime.UtcNow,
                UserId = user.Id,
                User = user,
            };

            await _repository.CreateAsync(comment);
            return Ok(comment.ToResponseDTO());            
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromBody] UpdateCommentDTO commentDTO, int id)
        {
            var user =await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }
            var comment = await _repository.GetByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            if (comment.UserId != user.Id)
            {
                return Forbid();
            }

            comment.Text = commentDTO.Text;

            await _repository.UpdateAsync(comment);

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

            var comment = await _repository.GetByIdAsync(id);

            if (comment == null)
            {
                return NotFound();
            }
            if (comment.UserId != user.Id)
            {
                return Forbid();
            }

            await _repository.DeleteAsync(comment.Id);

            return NoContent();
        }

    }
}
