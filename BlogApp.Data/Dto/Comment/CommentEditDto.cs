using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Dto.Comment
{
    public class CommentEditDto
    {
        [Required]
        public int CommentId { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Content must be at least 1 character.")]
        public string Content { get; set; } = null!;
    }
}
