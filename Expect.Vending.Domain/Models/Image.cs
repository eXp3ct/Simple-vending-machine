using Expect.Vending.Domain.Interfaces;
using Newtonsoft.Json;

namespace Expect.Vending.Domain.Models
{
    public class Image : IEntity
    {
        public Guid Id { get; set; }

    }
}
