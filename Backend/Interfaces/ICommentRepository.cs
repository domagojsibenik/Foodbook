using Foodbook.DTO;
using Foodbook.Helpers;
using Foodbook.Models;

namespace Foodbook.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetByIdAsync(int id);
        Task<Comment> CreateAsync(Comment comment);
        Task<Comment?> UpdateAsync(int id, UpdateCommentDTO commentDTO);
        Task<Comment?> DeleteAsync(int id);
        Task<bool> RecipeExists(int id);
    }
}
