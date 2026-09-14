using Foodbook.DTO;
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
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICommentRepository _repository;
        
        public CommentController(AppDbContext context, UserManager<AppUser> userManager, ICommentRepository repository)
        {
            _context = context;
            _userManager = userManager;
            _repository = repository;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var comment = await _repository.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _repository.GetAllAsync();
            return Ok(comments);
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

            var comment = new Comment()
            {
                Text = commentDTO.Text,
                RecipeId = commentDTO.RecipeId,
                CreatedAt = commentDTO.CreatedAt,
                UserId = user.Id
            };

            await _repository.CreateAsync(comment);
            return Ok(commentDTO);            
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromBody] UpdateCommentDTO commentDTO, int id)
        {
            var comment = await _repository.UpdateAsync(id, commentDTO);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _repository.DeleteAsync(id);

            if(comment == null)
            {
                return NotFound();
            }

            

            return Ok();
        }

    }
}
