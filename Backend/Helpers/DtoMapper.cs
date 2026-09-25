using Foodbook.DTO;
using Foodbook.Models;

namespace Foodbook.Helpers
{
    public static class DtoMapper
    {
        public static UserSummaryDTO ToSummaryDTO(this AppUser user)
        {
            return new UserSummaryDTO
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty
            };
        }

        public static CommentResponseDTO ToResponseDTO(this Comment comment)
        {
            return new CommentResponseDTO
            {
                Id = comment.Id,
                Text = comment.Text,
                CreatedAt = comment.CreatedAt,
                RecipeId = comment.RecipeId,
                User = comment.User.ToSummaryDTO()
            };
        }

        public static LikeResponseDTO ToResponseDTO(this Like like)
        {
            return new LikeResponseDTO
            {
                Id = like.Id,
                RecipeId = like.RecipeId,
                CreatedAt = like.CreatedAt,
                User = like.User.ToSummaryDTO()
            };
        }

        public static RecipeResponseDTO ToResponseDTO(
            this Recipe recipe)
        {
            return new RecipeResponseDTO
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                CookingTimeInMinutes = recipe.CookingTimeInMinutes,

                ImageUrl = recipe.ImageUrl,

                User = recipe.User.ToSummaryDTO(),

                Comments = recipe.Comments
                    .Select(c => c.ToResponseDTO())
                    .ToList(),

                Likes = recipe.Likes
                    .Select(l => l.ToResponseDTO())
                    .ToList()
            };
        }
    }
}