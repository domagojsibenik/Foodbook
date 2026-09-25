using System.ComponentModel.DataAnnotations;

namespace Foodbook.DTO
{
    public class UpdateCommentDTO
    {
        [Required]
        [MaxLength(1000)]
        public string Text { get; set; } = string.Empty;
    }
}