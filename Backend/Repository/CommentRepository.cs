using Foodbook.DTO;
using Foodbook.Helpers;
using Foodbook.Interfaces;
using Foodbook.Models;
using Foodbook.Services;
using Microsoft.EntityFrameworkCore;

namespace Foodbook.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return comment;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);

            if(comment == null)
            {
                return null;
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return comment;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            var comments = await _context.Comments.ToListAsync();
            return comments;
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (comment == null)
            {
                return null;
            }
            return comment;

        }

        public Task<bool> RecipeExists(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Comment?> UpdateAsync(int id, UpdateCommentDTO commentDTO)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (comment == null)
            {
                return null;
            }

            comment.Text = commentDTO.Text;
            await _context.SaveChangesAsync();

            return comment;
        }
    }
}
