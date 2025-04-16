using BlogApp.Data.Entities;

namespace BlogApp.Data.Interfaces
{
    public interface IArticleVoteRepository
    {
        public Task AddVoteAsync(ArticleVote articleVote);
        public Task UpdateVoteAsync(ArticleVote articleVote);
        public Task DeleteVoteAsync(ArticleVote articleVote);
        public Task<ArticleVote?> GetVoteAsync(int articleId, string? userId);
        public Task<int> GetArticlePositiveVotesCountAsync(int articleId);
        public Task<int> GetArticleNegativeVotesCountAsync(int articleId);
    }
}
