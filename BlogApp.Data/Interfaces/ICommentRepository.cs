using BlogApp.Data.Entities;
using BlogApp.Data.Helpers.Mapper;

namespace BlogApp.Data.Interfaces
{
    public interface ICommentRepository
    {
        /// <summary>
        /// Method to get paginated comments
        /// </summary>
        /// <param name="articleId">article id</param>
        /// <param name="pageIndex">page index</param>
        /// <param name="pageSize">page size</param>
        /// <returns>paginated article comments</returns>
        public Task<PaginatedList<Comment>> GetPaginatedCommentsAsync(int articleId, int pageIndex, int pageSize);

        /// <summary>
        /// Method to create comment
        /// </summary>
        /// <param name="comment">comment</param>
        public Task CreateCommentAsync(Comment comment);

        /// <summary>
        /// Method to edit comment
        /// </summary>
        /// <param name="comment">comment to edit</param>
        public Task EditCommentAsync(Comment comment);

        /// <summary>
        /// Method to delete comment
        /// </summary>
        /// <param name="comment">comment to delete</param>
        public Task DeleteCommentAsync(Comment comment);

        /// <summary>
        /// Method to get comment by id
        /// </summary>
        /// <param name="commentId">comment id</param>
        /// <returns>comment by id</returns>
        public Task<Comment?> GetCommentByIdAsync(int commentId);
    }
}
