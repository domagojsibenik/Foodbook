using Foodbook.DTO;
using Foodbook.Models;

namespace Foodbook.Interfaces
{
    public interface ILikeRepository
    {
        Task<Like?> getOneAsync(int id);
        Task<List<Like>> getAllAsync();
        Task<Like> createAsync(LikeDTO like, string userId);
        Task<Like?> deleteAsync(int id);
        Task<bool> doesExist(int recipeId, string userId);
    }
}
