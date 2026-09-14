using Foodbook.DTO;
using Foodbook.Helpers;
using Foodbook.Models;

namespace Foodbook.Interfaces
{
    public interface IRecipeRepository
    {
        Task<List<Recipe>> GetAllAsync(QueryObject query);
        Task<Recipe?> GetByIdAsync(int id);
        Task<Recipe> CreateAsync(Recipe recipe);
        Task<Recipe?> UpdateAsync(int id,RecipeDTO recipeDTO);
        Task<Recipe?> DeleteAsync(int id);
        Task<bool> RecipeExists(int id);
    }
}
