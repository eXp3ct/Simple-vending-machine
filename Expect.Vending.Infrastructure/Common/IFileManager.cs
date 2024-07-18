using Microsoft.AspNetCore.Http;

namespace Expect.Vending.Infrastructure.Common
{
    public interface IFileManager
    {
        public Task<bool> SaveImage(Guid imageId, IFormFile file, CancellationToken cancellationToken);
        public Task<byte[]?> GetImage(Guid id, CancellationToken cancellationToken);
        public Task<bool> DeleteImage(Guid id, CancellationToken cancellationToken);
    }
}
