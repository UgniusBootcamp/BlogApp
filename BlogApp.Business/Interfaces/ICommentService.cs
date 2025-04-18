using BlogApp.Data.Dto.Comment;
using BlogApp.Data.Helpers.Mapper;

namespace BlogApp.Business.Interfaces
{
    public interface ICommentService
    {
        /// <summary>
        /// Method to get paginated comments
        /// </summary>
        /// <param name="articleId">article id</param>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>paginated article comments</returns>
        public Task<PaginatedList<CommentReadDto>> GetArticleCommentsAsync(int articleId, int pageIndex, int pageSize);

        /// <summary>
        /// Method to create comment
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="createCommentDto">comment create dto</param>
        public Task CreateCommentAsync(string userId, CommentCreateDto createCommentDto);

        /// <summary>
        /// Method to edit comment
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="editCommentDto">comment edit dto</param>
        public Task EditCommentAsync(string userId, CommentEditDto editCommentDto);

        /// <summary>
        /// Method to delete comment
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="commentId">comment id</param>
        public Task DeleteCommentAsync(string userId, int commentId);
        
        /// <summary>
        /// Method to get comment by id 
        /// </summary>
        /// <param name="commentId">comment id</param>
        /// <returns>comment</returns>
        public Task<CommentReadDto> GetCommentByIdAsync(int commentId);
    }
}
