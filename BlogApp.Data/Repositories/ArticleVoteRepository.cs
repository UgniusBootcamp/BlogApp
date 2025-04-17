using BlogApp.Data.Data;
using BlogApp.Data.Entities;
using BlogApp.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Repositories
{
    public class ArticleVoteRepository(BlogAppDbContext context) : IArticleVoteRepository
    {
        /// <summary>
        /// Method to add article vote
        /// </summary>
        /// <param name="articleVote">article vote to add</param>
        public async Task AddVoteAsync(ArticleVote articleVote)
        {
            context.ArticleVotes.Add(articleVote);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Method to delete article vote
        /// </summary>
        /// <param name="articleVote">article vote to delete</param>
        public async Task DeleteVoteAsync(ArticleVote articleVote)
        {
            context.ArticleVotes.Remove(articleVote);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Method to get negative article vote count
        /// </summary>
        /// <param name="articleId">article id</param>
        /// <returns>negative article vote count</returns>
        public async Task<int> GetArticleNegativeVotesCountAsync(int articleId)
        {
            return await context.ArticleVotes
                .CountAsync(v => v.ArticleId == articleId && v.VoteValue == false);
        }

        /// <summary>
        /// Method to get positive article vote count
        /// </summary>
        /// <param name="articleId">article id</param>
        /// <returns>positive article vote count</returns>
        public async Task<int> GetArticlePositiveVotesCountAsync(int articleId)
        {
            return await context.ArticleVotes
                .CountAsync(v => v.ArticleId == articleId && v.VoteValue == true);
        }

        /// <summary>
        /// Method to get article vote
        /// </summary>
        /// <param name="articleId">article id</param>
        /// <param name="userId">user id</param>
        /// <returns>article vote</returns>
        public async Task<ArticleVote?> GetVoteAsync(int articleId, string? userId)
        {
            return await context.ArticleVotes
                 .FirstOrDefaultAsync(v => v.ArticleId == articleId && v.UserId == userId);
        }

        /// <summary>
        /// Method to update article vote
        /// </summary>
        /// <param name="articleVote">article vote to update</param>
        public async Task UpdateVoteAsync(ArticleVote articleVote)
        {
            context.ArticleVotes.Update(articleVote);
            await context.SaveChangesAsync();
        }
    }
}
