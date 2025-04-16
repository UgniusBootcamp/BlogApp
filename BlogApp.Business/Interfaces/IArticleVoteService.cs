using BlogApp.Data.Dto.ArticleVote;

namespace BlogApp.Business.Interfaces
{
    public interface IArticleVoteService
    {
        public Task VoteAsync(string userId, ArticleVoteCreateDto articleVoteCreateDto);
        public Task<ArticleVoteReadDto> GetArticleVotesAsync(int articleId, string? userId);
    }
}
