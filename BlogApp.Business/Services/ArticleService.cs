﻿using AutoMapper;
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
        IMapper mapper
        ) : IArticleService
    {
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

        public async Task DeleteArticleAsync(string userId, int id)
        {
            var article = await articleRepository.GetArticleAsync(id);

            if (article == null)
                throw new NotFoundException(ServiceConstants.ArticleNotFound);

            if(article.UserId != userId)
                throw new ForbiddenException(ServiceConstants.ArticleNotBelongsToUser);

            await articleRepository.DeleteArticleAsync(article);
        }

        public async Task<ArticleDetailDto> GetArticleAsync(int id)
        {
            var article = await articleRepository.GetArticleAsync(id);

            if (article == null)
                throw new NotFoundException(ServiceConstants.ArticleNotFound);

            return mapper.Map<ArticleDetailDto>(article);
        }

        public async Task<PaginatedList<ArticleListDto>> GetArticlesAsync(int pageIndex, int pageSize, string? userId = null)
        {
            var articles = await articleRepository.GetArticlesAsync(pageIndex, pageSize, userId);

            return new PaginatedList<ArticleListDto>(mapper.Map<List<ArticleListDto>>(articles.Items), articles.PageIndex, articles.TotalPages);
        }

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
