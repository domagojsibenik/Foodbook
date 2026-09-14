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

        public async Task<Recipe?> DeleteAsync(int id)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(x => x.Id == id);

            if (recipe == null)
            {
                return null;
            }

            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();

            return recipe;
        }

        public async Task<List<Recipe>> GetAllAsync(QueryObject query)
        {
            var recipes = _context.Recipes.AsQueryable();

            if (query.UserId != null)
            {
                recipes = recipes.Where(s => s.UserId == query.UserId);
            }

            return await recipes.ToListAsync();
        }

        public async Task<Recipe?> GetByIdAsync(int id)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(r => r.Id == id);
            return recipe;
        }

        public Task<bool> RecipeExists(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Recipe?> UpdateAsync(int id, RecipeDTO recipeDTO)
        {
            var recipe = await _context.Recipes.FirstOrDefaultAsync(x => x.Id == id);
            recipe.Name = recipeDTO.Name;
            recipe.Description = recipeDTO.Description;
            recipe.CookingTimeInMinutes = recipeDTO.CookingTimeInMinutes;
            await _context.SaveChangesAsync();

            return recipe;
        }
    }
}
