using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Dto.Article
{
    public class ArticleCreateDto
    {
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(50, ErrorMessage = "Title cannot exceed 50 characters")]
        [Display(Name = "Title")]
        public string Title { get; set; } = null!;

        [Display(Name = "Content")]
        public string? Content { get; set; }

        [Display(Name = "Featured Image (JPG, PNG, GIF only)")]
        public IFormFile? Image { get; set; }
    }
}
