using AutoMapper;
using BlogApp.Business.Interfaces;
using BlogApp.Data.Dto.ArticleVote;
using BlogApp.Data.Entities;
using BlogApp.Data.Interfaces;

namespace BlogApp.Business.Services
{
    public class ArticleVoteService(
        IArticleVoteRepository articleVoteRepository,
        IMapper mapper
        ) : IArticleVoteService
    {
        public async Task<ArticleVoteReadDto> GetArticleVotesAsync(int articleId, string? userId)
        {
            var articleVoteReadDto = new ArticleVoteReadDto();

            var articleVote = await articleVoteRepository.GetVoteAsync(articleId, userId);

            if (articleVote != null)
            {
                articleVoteReadDto = mapper.Map<ArticleVoteReadDto>(articleVote);
            }
            else
            {
                articleVoteReadDto.ArticleId = articleId;
            }

            articleVoteReadDto.PositiveVotes = await articleVoteRepository.GetArticlePositiveVotesCountAsync(articleId);
            articleVoteReadDto.NegativeVotes = await articleVoteRepository.GetArticleNegativeVotesCountAsync(articleId);

            return articleVoteReadDto;
        }

        public async Task VoteAsync(string userId, ArticleVoteCreateDto articleVoteCreateDto)
        {
            var articleVote = await articleVoteRepository.GetVoteAsync(articleVoteCreateDto.ArticleId, userId);

            if(articleVote == null)
            {
                var newArticleVote = mapper.Map<ArticleVote>(articleVoteCreateDto);
                newArticleVote.UserId = userId;
                await articleVoteRepository.AddVoteAsync(newArticleVote);
            }
            else
            {
                var voteValue = articleVote.VoteValue;

                if(voteValue == articleVoteCreateDto.VoteValue)
                {
                    await articleVoteRepository.DeleteVoteAsync(articleVote);
                }
                else
                {
                    articleVote.VoteValue = articleVoteCreateDto.VoteValue;
                    await articleVoteRepository.UpdateVoteAsync(articleVote);
                }
            }
        }
    }
}
