﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Data.Entities
{
    public class ArticleVote
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [Required]
        public bool VoteValue { get; set; }

        [Required]
        public int ArticleId { get; set; }
        public Article Article { get; set; } = null!;

        [Required]
        public string UserId { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
