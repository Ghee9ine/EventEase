using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace EventEase.Services
{
    public class BlobService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("BlobStorage");
            var blobServiceClient = new BlobServiceClient(connectionString);
            _containerClient = blobServiceClient.GetBlobContainerClient("venue-images");
            
            // Create container if it doesn't exist
            _containerClient.CreateIfNotExists();
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var blobClient = _containerClient.GetBlobClient(fileName);
            
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });
            }
            
            return blobClient.Uri.ToString();
        }

        public async Task DeleteImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;
            
            var uri = new Uri(imageUrl);
            string blobName = Path.GetFileName(uri.LocalPath);
            var blobClient = _containerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();
        }
    }
}