using Microsoft.AspNetCore.Http;

namespace BlogApp.Business.Interfaces
{
    public interface IBlobService
    {
        /// <summary>
        /// Method to save file to blob storage
        /// </summary>
        /// <param name="file">file</param>
        /// <param name="articleId">article id</param>
        /// <returns>saved file url</returns>
        public Task<string> SaveImageAsync(IFormFile file, int articleId);

        /// <summary>
        /// Method to delete file from blob storage
        /// </summary>
        /// <param name="imageUrl">image url</param>
        public Task DeleteImageAsync(string? imageUrl);
    }
}
