using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Dto.Comment
{
    public class CommentCreateDto
    {
        [Required]
        public int ArticleId { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Content must be at least 1 character.")]
        public string Content { get; set; } = null!;
    }
}
