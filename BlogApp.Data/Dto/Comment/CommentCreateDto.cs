using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Dto.Comment
{
    public class CommentCreateDto
    {
        [Required]
        public int ArticleId { get; set; }

        [Required]
        [MaxLength(500, ErrorMessage = "Comment cannot exceed 500 characters")]
        public string Content { get; set; } = null!;
    }
}
