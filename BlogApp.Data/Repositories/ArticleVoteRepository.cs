using BlogApp.Data.Data;
using BlogApp.Data.Entities;
using BlogApp.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Repositories
{
    public class ArticleVoteRepository(BlogAppDbContext context) : IArticleVoteRepository
    {
        public async Task AddVoteAsync(ArticleVote articleVote)
        {
            context.ArticleVotes.Add(articleVote);
            await context.SaveChangesAsync();
        }

        public async Task DeleteVoteAsync(ArticleVote articleVote)
        {
            context.ArticleVotes.Remove(articleVote);
            await context.SaveChangesAsync();
        }

        public async Task<int> GetArticleNegativeVotesCountAsync(int articleId)
        {
            return await context.ArticleVotes
                .CountAsync(v => v.ArticleId == articleId && v.VoteValue == false);
        }

        public async Task<int> GetArticlePositiveVotesCountAsync(int articleId)
        {
            return await context.ArticleVotes
                .CountAsync(v => v.ArticleId == articleId && v.VoteValue == true);
        }

        public async Task<ArticleVote?> GetVoteAsync(int articleId, string? userId)
        {
            return await context.ArticleVotes
                 .FirstOrDefaultAsync(v => v.ArticleId == articleId && v.UserId == userId);
        }

        public async Task UpdateVoteAsync(ArticleVote articleVote)
        {
            context.ArticleVotes.Update(articleVote);
            await context.SaveChangesAsync();
        }
    }
}
