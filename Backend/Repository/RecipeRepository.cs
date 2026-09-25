using Foodbook.DTO;
using Foodbook.Helpers;
using Foodbook.Interfaces;
using Foodbook.Models;
using Foodbook.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Foodbook.Repository
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly AppDbContext _context;

        public RecipeRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Recipe> CreateAsync(Recipe recipe)
        {
            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();

            return recipe;
        }

        public async Task<Recipe?> DeleteAsync(int recipeid)
        {
            var findrecipe = await _context.Recipes.FindAsync(recipeid);

            if (findrecipe == null)
            {
                return null;
            }

            _context.Recipes.Remove(findrecipe);
            await _context.SaveChangesAsync();

            return findrecipe;
        }

        public async Task<List<Recipe>> GetAllAsync(QueryObject query)
        {
            var recipes = _context.Recipes
               .AsNoTracking()
               .Include(r => r.User)
               .Include(r => r.Comments)
                   .ThenInclude(c => c.User)
               .Include(r => r.Likes)
                   .ThenInclude(l => l.User)
               .AsSplitQuery();
              

            if (!string.IsNullOrWhiteSpace(query.UserId))
            {
                recipes = recipes.Where(
                    r => r.UserId == query.UserId);
            }

            return await recipes.ToListAsync();
        }

        public async Task<Recipe?> GetByIdAsync(int id)
        {
            return await _context.Recipes
            .AsNoTracking()
            .Include(r => r.User)
            .Include(r => r.Comments)
                .ThenInclude(c => c.User)
            .Include(r => r.Likes)
                .ThenInclude(l => l.User)
            .AsSplitQuery()
            .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Recipes.AnyAsync(r => r.Id == id);
        }

        public async Task<Recipe?> UpdateAsync(Recipe recipe)
        {

            var updatedRecipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == recipe.Id);

            if (updatedRecipe == null)
            {
                return null;
            }

            updatedRecipe.Name = recipe.Name;
            updatedRecipe.Description = recipe.Description;
            updatedRecipe.CookingTimeInMinutes = recipe.CookingTimeInMinutes;
            updatedRecipe.ImageUrl = recipe.ImageUrl;
            await _context.SaveChangesAsync();

            return updatedRecipe;
        }
    }
}
