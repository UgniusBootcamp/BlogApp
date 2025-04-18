using BlogApp.Data.Dto.Comment;
using BlogApp.Data.Helpers.Mapper;

namespace BlogApp.Business.Interfaces
{
    public interface ICommentService
    {
        public Task<PaginatedList<CommentReadDto>> GetArticleCommentsAsync(int articleId, int pageIndex, int pageSize);
        public Task CreateCommentAsync(string userId, CommentCreateDto createCommentDto);
        public Task EditCommentAsync(string userId, CommentEditDto editCommentDto);
        public Task DeleteCommentAsync(string userId, int commentId);
        public Task<CommentReadDto> GetCommentByIdAsync(int commentId);
    }
}
