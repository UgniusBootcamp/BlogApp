using AutoMapper;
using BlogApp.Business.Interfaces;
using BlogApp.Data.Constants;
using BlogApp.Data.Dto.Comment;
using BlogApp.Data.Entities;
using BlogApp.Data.Helpers.Exceptions;
using BlogApp.Data.Helpers.Mapper;
using BlogApp.Data.Interfaces;

namespace BlogApp.Business.Services
{
    public class CommentService(
        ICommentRepository commentRepository,
        IMapper mapper
        ) : ICommentService
    {
        public async Task CreateCommentAsync(string userId, CommentCreateDto createCommentDto)
        {
            var comment = mapper.Map<Comment>(createCommentDto);

            comment.UserId = userId;
            comment.CreatedAt = DateTime.UtcNow;

            await commentRepository.CreateCommentAsync(comment);
        }

        public async Task DeleteCommentAsync(string userId, int commentId)
        {
            var comment = await commentRepository.GetCommentByIdAsync(commentId);

            if(comment == null)
                throw new NotFoundException(ServiceConstants.CommentNotFound);

            if (comment.UserId != userId)
                throw new ForbiddenException(ServiceConstants.CommentNotBelongsToUser);

            await commentRepository.DeleteCommentAsync(comment);
        }

        public async Task EditCommentAsync(string userId, CommentEditDto editCommentDto)
        {
            var comment = await commentRepository.GetCommentByIdAsync(editCommentDto.CommentId);

            if (comment == null)
                throw new NotFoundException(ServiceConstants.CommentNotFound);

            if (comment.UserId != userId)
                throw new ForbiddenException(ServiceConstants.CommentNotBelongsToUser);

            mapper.Map(editCommentDto, comment);

            await commentRepository.EditCommentAsync(comment);
        }

        public async Task<PaginatedList<CommentReadDto>> GetArticleCommentsAsync(int articleId, int pageIndex, int pageSize)
        {
            var comments = await commentRepository.GetPaginatedCommentsAsync(articleId, pageIndex, pageSize);

            return new PaginatedList<CommentReadDto>(mapper.Map<IEnumerable<CommentReadDto>>(comments.Items), comments.PageIndex, comments.TotalPages);
        }

        public async Task<CommentReadDto> GetCommentByIdAsync(int commentId)
        {
            var comment = await commentRepository.GetCommentByIdAsync(commentId);

            if (comment == null)
                throw new NotFoundException(ServiceConstants.CommentNotFound);

            return mapper.Map<CommentReadDto>(comment);
        }
    }
}
