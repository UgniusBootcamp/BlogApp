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

        /// <summary>
        /// Method to create comment
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="createCommentDto">comment create dto</param>
        public async Task CreateCommentAsync(string userId, CommentCreateDto createCommentDto)
        {
            var comment = mapper.Map<Comment>(createCommentDto);

            comment.UserId = userId;
            comment.CreatedAt = DateTime.UtcNow;

            await commentRepository.CreateCommentAsync(comment);
        }

        /// <summary>
        /// Method to delete comment
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="commentId">comment id</param>
        /// <exception cref="NotFoundException">if comment not found</exception>
        /// <exception cref="ForbiddenException">if comment does not belong to user</exception>
        public async Task DeleteCommentAsync(string userId, int commentId)
        {
            var comment = await commentRepository.GetCommentByIdAsync(commentId);

            if(comment == null)
                throw new NotFoundException(ServiceConstants.CommentNotFound);

            if (comment.UserId != userId)
                throw new ForbiddenException(ServiceConstants.CommentNotBelongsToUser);

            await commentRepository.DeleteCommentAsync(comment);
        }

        /// <summary>
        /// Method to edit comment
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="editCommentDto">edit comment dto</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">if comment not found</exception>
        /// <exception cref="ForbiddenException">if comment does not belong to user</exception>
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

        /// <summary>
        /// Method to get paginated article commnents
        /// </summary>
        /// <param name="articleId">article id</param>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>paginated article comments</returns>
        public async Task<PaginatedList<CommentReadDto>> GetArticleCommentsAsync(int articleId, int pageIndex, int pageSize)
        {
            var comments = await commentRepository.GetPaginatedCommentsAsync(articleId, pageIndex, pageSize);

            return new PaginatedList<CommentReadDto>(mapper.Map<IEnumerable<CommentReadDto>>(comments.Items), comments.PageIndex, comments.TotalPages);
        }
        
        /// <summary>
        /// Method to get comment by id
        /// </summary>
        /// <param name="commentId">comment id</param>
        /// <returns>comment</returns>
        /// <exception cref="NotFoundException">if comment not found</exception>
        public async Task<CommentReadDto> GetCommentByIdAsync(int commentId)
        {
            var comment = await commentRepository.GetCommentByIdAsync(commentId);

            if (comment == null)
                throw new NotFoundException(ServiceConstants.CommentNotFound);

            return mapper.Map<CommentReadDto>(comment);
        }

        /// <summary>
        /// Method to get last article comment
        /// </summary>
        /// <param name="articleId">article id</param>
        /// <returns>last article commment</returns>
        public async Task<CommentReadDto?> GetLastArticleCommentByIdAsync(int articleId)
        {
            var comment = await commentRepository.GetLastArticleCommentByIdAsync(articleId);

            return mapper.Map<CommentReadDto>(comment);
        }
    }
}
