using Microsoft.AspNetCore.Http;

namespace Expect.Vending.Domain.Dtos
{
    public class ImageDto
    {
        public Guid DrinkId { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
