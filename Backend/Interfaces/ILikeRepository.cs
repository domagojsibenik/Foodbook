using Foodbook.DTO;
using Foodbook.Models;

namespace Foodbook.Interfaces
{
    public interface ILikeRepository
    {
        Task<Like?> GetByIdAsync(int id);
        Task<List<Like>> GetAllAsync();
        Task<Like> CreateAsync(Like like);
        Task<Like?> DeleteAsync(int id);
        Task<bool> ExistsAsync(int recipeId, string userId);
    }
}
