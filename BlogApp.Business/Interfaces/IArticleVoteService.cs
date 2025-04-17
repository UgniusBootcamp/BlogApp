using BlogApp.Data.Dto.ArticleVote;

namespace BlogApp.Business.Interfaces
{
    public interface IArticleVoteService
    {
        /// <summary>
        /// Method to for user to vote for article
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="articleVoteCreateDto">article vote create dto</param>
        public Task VoteAsync(string userId, ArticleVoteCreateDto articleVoteCreateDto);

        /// <summary>
        /// Method to get article votes
        /// </summary>
        /// <param name="articleId">article id</param>
        /// <param name="userId">user id</param>
        /// <returns>article votes</returns>
        public Task<ArticleVoteReadDto> GetArticleVotesAsync(int articleId, string? userId);
    }
}
