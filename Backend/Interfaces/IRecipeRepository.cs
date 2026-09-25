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
        Task<Recipe?> UpdateAsync(Recipe recipe);
        Task<Recipe?> DeleteAsync(int recipeid);
        Task<bool> ExistsAsync(int id);
    }
}
