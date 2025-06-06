﻿using BlogApp.Data.Dto.User;

namespace BlogApp.Data.Dto.Article
{
    public class ArticleTagDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public UserDetailDto User { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
