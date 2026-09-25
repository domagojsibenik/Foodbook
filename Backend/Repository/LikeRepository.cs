using Foodbook.DTO;
using Foodbook.Helpers;
using Foodbook.Interfaces;
using Foodbook.Models;
using Foodbook.Services;
using Microsoft.EntityFrameworkCore;

namespace Foodbook.Repository
{
    public class LikeRepository : ILikeRepository
    {
        private readonly AppDbContext _context;
        public LikeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Like> CreateAsync(Like like)
        {
            await _context.Likes.AddAsync(like);
            await _context.SaveChangesAsync();

            return like;
        }

        public async Task<Like?> DeleteAsync(int id)
        {
            var like = await _context.Likes.FirstOrDefaultAsync(x => x.Id == id);

            if (like == null)
            {
                return null;
            }

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();

            return like;

        }

        public async Task<List<Like>> GetAllAsync()
        {
            var recipes = await _context.Likes.AsNoTracking().Include(c => c.User).ToListAsync();
            return recipes;
        }

        public async Task<Like?> GetByIdAsync(int id)
        {
            var recipe = await _context.Likes.AsNoTracking().Include(c => c.User).FirstOrDefaultAsync(x => x.Id == id);
            return recipe;
        }

        public async Task<bool>  ExistsAsync(int recipeId, string userId)
        {
            var likeCheck = await _context.Likes.AnyAsync(x => x.UserId == userId && x.RecipeId == recipeId);

            if (likeCheck)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
