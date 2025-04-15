using Microsoft.AspNetCore.Http;

namespace BlogApp.Business.Interfaces
{
    public interface IBlobService
    {
        public Task<string> SaveImageAsync(IFormFile file, int articleId);
        public Task DeleteImageAsync(string? imageUrl);
    }
}
