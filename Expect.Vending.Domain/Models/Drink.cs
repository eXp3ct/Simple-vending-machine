using Expect.Vending.Domain.Interfaces;
using Newtonsoft.Json;

namespace Expect.Vending.Domain.Models
{
    public class Drink : IEntity
    {
        public Guid Id { get; set; }
        public Guid? ImageId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

    }
}
