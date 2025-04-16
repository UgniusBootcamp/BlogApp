using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Dto.Article
{
    public class ArticleUpdateDto
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [MaxLength(50, ErrorMessage = "Title cannot exceed 50 characters")]
        public string Title { get; set; } = null!;

        public string? Content { get; set; }

        public string? ImageUrl { get; set; }

        public IFormFile? Image { get; set; }
        public bool HasChanged { get; set; } = false;
    }
}
