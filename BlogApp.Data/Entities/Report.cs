using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Entities
{
    public class Report
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;

        [Required]
        public int CommentId { get; set; }
        public Comment Comment { get; set; } = null!;
    }
}
