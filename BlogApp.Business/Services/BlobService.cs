using Azure.Storage.Blobs;
using BlogApp.Business.Interfaces;
using BlogApp.Data.Constants;
using BlogApp.Data.Helpers.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace BlogApp.Business.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobContainerClient _blobContainerClient;
        private readonly string[] allowedTypes = ServiceConstants.AllowedTypes;
        private readonly string[] allowedExtensions = ServiceConstants.AllowedExtensions;

        public BlobService(IOptions<AzureBlobServiceConfiguration> options)
        {
            _blobContainerClient = new BlobContainerClient(options.Value.Key, options.Value.ContainerName);
            _blobContainerClient.CreateIfNotExists();
        }

        /// <summary>
        /// Method to delete blob
        /// </summary>
        /// <param name="imageUrl">image url</param>
        public Task DeleteImageAsync(string? imageUrl)
        {
            if (!string.IsNullOrEmpty(imageUrl))
            {
                var blobName = Path.GetFileName(imageUrl);
                BlobClient blobClient = _blobContainerClient.GetBlobClient(blobName);

                return blobClient.DeleteIfExistsAsync();
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Method to save file to blob storage
        /// </summary>
        /// <param name="file">file</param>
        /// <param name="articleId">article id</param>
        /// <exception cref="InvalidOperationException">wrong file extension or type</exception>
        public async Task<string> SaveImageAsync(IFormFile file, int articleId)
        {
            if (!allowedTypes.Contains(file.ContentType.ToLower()))
                throw new InvalidOperationException(ServiceConstants.AvailableFileTypes);

            var extension = Path.GetExtension(file.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
                throw new InvalidOperationException(ServiceConstants.AvailableFileTypes);

            var blobName = $"{articleId}{extension}";

            BlobClient blobClient = _blobContainerClient.GetBlobClient(blobName);

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            return blobClient.Uri.ToString();
        }
    }
}
