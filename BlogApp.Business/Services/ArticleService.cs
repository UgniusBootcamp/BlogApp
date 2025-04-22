using AutoMapper;
using BlogApp.Business.Interfaces;
using BlogApp.Data.Constants;
using BlogApp.Data.Dto.Article;
using BlogApp.Data.Entities;
using BlogApp.Data.Helpers.Exceptions;
using BlogApp.Data.Helpers.Mapper;
using BlogApp.Data.Interfaces;

namespace BlogApp.Business.Services
{
    public class ArticleService(
        IArticleRepository articleRepository,
        IBlobService blobService,
        ICommentService commentService,
        IArticleVoteService articleVoteService,
        IMapper mapper
        ) : IArticleService
    {
        /// <summary>
        /// Method to create article
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="articleCreateDto">article create dto</param>
        public async Task CreateArticleAsync(string userId, ArticleCreateDto articleCreateDto)
        {
            var article = mapper.Map<Article>(articleCreateDto);

            article.UserId = userId;
            article.CreatedAt = DateTime.UtcNow;

            await articleRepository.CreateArticleAsync(article);

            if (articleCreateDto.Image != null)
            {
                var imageUrl = await blobService.SaveImageAsync(articleCreateDto.Image, article.Id);
                article.ImageUrl = imageUrl;
                await articleRepository.UpdateArticleAsync(article);
            }
        }

        /// <summary>
        /// Method to delete article
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="id">article id</param>
        /// <exception cref="NotFoundException"> if article not found</exception>
        /// <exception cref="ForbiddenException">article does not belong to user</exception>
        public async Task DeleteArticleAsync(string userId, int id)
        {
            var article = await articleRepository.GetArticleAsync(id);

            if (article == null)
                throw new NotFoundException(ServiceConstants.ArticleNotFound);

            if(article.UserId != userId)
                throw new ForbiddenException(ServiceConstants.ArticleNotBelongsToUser);

            await articleRepository.DeleteArticleAsync(article);
        }

        /// <summary>
        /// Method to get article by id
        /// </summary>
        /// <param name="id">article id</param>
        /// <returns>artilce</returns>
        /// <exception cref="NotFoundException">if article not found</exception>
        public async Task<ArticleDetailDto> GetArticleAsync(int id)
        {
            var article = await articleRepository.GetArticleAsync(id);

            if (article == null)
                throw new NotFoundException(ServiceConstants.ArticleNotFound);

            return mapper.Map<ArticleDetailDto>(article);
        }

        /// <summary>
        /// Method to get paginated list of articles
        /// </summary>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <param name="userId">user id</param>
        /// <returns>paginated list of articles</returns>
        public async Task<PaginatedList<ArticleListDto>> GetArticlesAsync(int pageIndex, int pageSize, string? userId = null)
        {
            var articles = await articleRepository.GetArticlesAsync(pageIndex, pageSize, userId);

            return new PaginatedList<ArticleListDto>(mapper.Map<List<ArticleListDto>>(articles.Items), articles.PageIndex, articles.TotalPages);
        }

        public async Task<IEnumerable<ArticleTagDto>> GetArticleTagsAsync(string? searchString, int count)
        {
            var articles = await articleRepository.GetArticlesAsync(searchString, count);

            return mapper.Map<IEnumerable<ArticleTagDto>>(articles);
        }

        public async Task<IEnumerable<ArticleListDto>> GetTopArticlesAsync(int count)
        {
            var articles = await articleRepository.GetTopArticlesAsync(count);

            var mapped = mapper.Map<List<ArticleListDto>>(articles);

            foreach (var article in mapped)
                article.Vote = await articleVoteService.GetArticleVotesAsync(article.Id, null);

            return mapped;
        }

        public async Task<IEnumerable<ArticleListDto>> LastArticlesAsync(int count)
        {
            var articles = await articleRepository.LastArticlesAsync(count);

            return mapper.Map<List<ArticleListDto>>(articles);
        }

        public async Task<IEnumerable<ArticleWithCommentDto>> LastCommentedArticlesAsync(int count)
        {
            var articles = await articleRepository.LastCommentedArticlesAsync(count);

            var mapped = mapper.Map<IEnumerable<ArticleWithCommentDto>>(articles);

            foreach (var article in mapped)
                article.LastComment = await commentService.GetLastArticleCommentByIdAsync(article.Id);

            return mapped.Where(a => a.LastComment != null);
        }

        /// <summary>
        /// Method to update article
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="articleUpdateDto">article update dto</param>
        /// <exception cref="NotFoundException">if article not found</exception>
        /// <exception cref="ForbiddenException">article does not belong to user</exception>
        public async Task UpdateArticleAsync(string userId, ArticleUpdateDto articleUpdateDto)
        {
            var article = await articleRepository.GetArticleAsync(articleUpdateDto.Id);

            if (article == null)
                throw new NotFoundException(ServiceConstants.ArticleNotFound);

            if (article.UserId != userId)
                throw new ForbiddenException(ServiceConstants.ArticleNotBelongsToUser);

            if(articleUpdateDto.HasChanged)
            {
                if(articleUpdateDto.Image == null)
                {
                    await blobService.DeleteImageAsync(article.ImageUrl);
                    articleUpdateDto.ImageUrl = null;
                } 
                else
                {
                    var imageUrl = await blobService.SaveImageAsync(articleUpdateDto.Image, article.Id);
                    articleUpdateDto.ImageUrl = imageUrl;
                }
            }

            mapper.Map(articleUpdateDto, article);

            await articleRepository.UpdateArticleAsync(article);
        }
    }
}
