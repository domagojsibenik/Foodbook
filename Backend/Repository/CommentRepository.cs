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
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return null;
            }

            _context.Comments.Remove(comment);

            await _context.SaveChangesAsync();

            return comment;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            var comments = await _context.Comments.AsNoTracking().Include(c => c.User).ToListAsync();
            return comments;
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            var comment = await _context.Comments.Include(c => c.User).FirstOrDefaultAsync(x => x.Id == id);
            if (comment == null)
            {
                return null;
            }
            return comment;

        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Comments.AnyAsync(r => r.Id == id);
        }

        public async Task<Comment?> UpdateAsync(Comment comment)
        {
            var _comment = await _context.Comments.FindAsync(comment.Id);
            if (_comment == null)
            {
                return null;
            }

            _comment.Text = comment.Text;
            await _context.SaveChangesAsync();

            return _comment;
        }
    }
}
