using BlogApp.Data.Entities;

namespace BlogApp.Data.Interfaces
{
    public interface IArticleVoteRepository
    {
        /// <summary>
        /// Method to add article vote
        /// </summary>
        /// <param name="articleVote">article vote</param>
        public Task AddVoteAsync(ArticleVote articleVote);

        /// <summary>
        /// Method to update article vote
        /// </summary>
        /// <param name="articleVote">article vote</param>
        public Task UpdateVoteAsync(ArticleVote articleVote);

        /// <summary>
        /// Method to delete article vote
        /// </summary>
        /// <param name="articleVote">article vote</param>
        public Task DeleteVoteAsync(ArticleVote articleVote);

        /// <summary>
        /// Method to get article vote
        /// </summary>
        /// <param name="articleId">article id</param>
        /// <param name="userId">user id</param>
        /// <returns>article vote</returns>
        public Task<ArticleVote?> GetVoteAsync(int articleId, string? userId);

        /// <summary>
        /// Method to get positive article vote count
        /// </summary>
        /// <param name="articleId">article id</param>
        /// <returns>positive article vote count</returns>
        public Task<int> GetArticlePositiveVotesCountAsync(int articleId);

        /// <summary>
        /// Method to get negative article vote count
        /// </summary>
        /// <param name="articleId">article id</param>
        /// <returns>negative article vote count</returns>
        public Task<int> GetArticleNegativeVotesCountAsync(int articleId);
    }
}
