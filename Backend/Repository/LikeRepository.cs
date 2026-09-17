using Foodbook.DTO;
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

        public async Task<Like> createAsync(LikeDTO like, string userId)
        {
            var likeModel = new Like()
            {
                UserId = userId,
                RecipeId = like.RecipeId,
                CreatedAt = like.CreatedAt
            };

            await _context.Likes.AddAsync(likeModel);
            await _context.SaveChangesAsync();

            return likeModel;
        }

        public async Task<Like?> deleteAsync(int id)
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

        public async Task<List<Like>> getAllAsync()
        {
            var recipes = await _context.Likes.ToListAsync();
            return recipes;
        }

        public async Task<Like?> getOneAsync(int id)
        {
            var recipe = await _context.Likes.FirstOrDefaultAsync(x => x.Id == id);
            return recipe;
        }

        public async Task<bool> doesExist(int recipeId, string userId)
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
