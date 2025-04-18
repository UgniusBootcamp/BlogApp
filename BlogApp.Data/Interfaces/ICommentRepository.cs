using BlogApp.Data.Entities;
using BlogApp.Data.Helpers.Mapper;

namespace BlogApp.Data.Interfaces
{
    public interface ICommentRepository
    {
        public Task<PaginatedList<Comment>> GetPaginatedCommentsAsync(int articleId, int pageIndex, int pageSize);
        public Task CreateCommentAsync(Comment comment);
        public Task EditCommentAsync(Comment comment);
        public Task DeleteCommentAsync(Comment comment);
        public Task<Comment?> GetCommentByIdAsync(int commentId);
    }
}
