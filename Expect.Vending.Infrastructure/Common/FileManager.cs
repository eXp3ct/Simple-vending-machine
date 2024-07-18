using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Expect.Vending.Infrastructure.Common
{
    public class FileManager : IFileManager
    {
        private string? _basePath;
        private readonly ILogger<FileManager> _logger;

        public FileManager(IConfiguration configuration, ILogger<FileManager> logger)
        {
            if (!EnsureDirectoryExist(configuration)) throw new DirectoryNotFoundException("Cannot find or create directory");
            _logger = logger;
        }

        public async Task<bool> DeleteImage(Guid id, CancellationToken cancellationToken)
        {
            var fileName = $"{id}.jpg";
            var filePath = Path.Combine(_basePath, fileName);

            if (!File.Exists(filePath))
            {
                _logger.LogWarning("Cannot find image {id}", id);
                return false;
            }

            await Task.Run(() => File.Delete(filePath), cancellationToken);
            _logger.LogInformation("Image {id} was deleted from local storage", id);
            return true;
        }

        public async Task<byte[]?> GetImage(Guid id, CancellationToken cancellationToken)
        {
            var fileName = $"{id}.jpg";
            var filePath = Path.Combine(_basePath, fileName);

            if (!File.Exists(filePath))
            {
                _logger.LogWarning("Cannot find image {id} on local storage", id);
                return null;
            }

            var fileBytes = await File.ReadAllBytesAsync(filePath, cancellationToken);
            _logger.LogInformation("Returning image {id} from local storage", id);
            return fileBytes;
        }

        public async Task<bool> SaveImage(Guid imageId, IFormFile file, CancellationToken cancellationToken)
        {
            if (file.Length <= 0)
            {
                _logger.LogWarning("Incorrect image {imageId}, cannot save on local storage", imageId);
                return false;
            }

            var fullName = file.FileName.Split('.');
            var extenstion = fullName.LastOrDefault();
            
            if(extenstion != "jpg")
            {
                _logger.LogWarning("Incorrect image type {id}, cannot save on local storage", imageId);
                return false;
            }

            var fileName = $"{imageId}.{extenstion}";
            var filePath = Path.Combine(_basePath, fileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream, cancellationToken);
            _logger.LogInformation("Image {id} saved on local storage", imageId);
            return true;
        }

        private bool EnsureDirectoryExist(IConfiguration configuration)
        {
            var basePath = configuration.GetSection("LocalStorage").Value;

            if (basePath is null) return false;

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            _basePath = basePath;
            return true;
        }
    }
}
