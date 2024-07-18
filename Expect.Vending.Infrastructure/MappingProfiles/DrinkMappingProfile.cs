using AutoMapper;
using Expect.Vending.Domain.Dtos;
using Expect.Vending.Domain.Models;

namespace Expect.Vending.Infrastructure.MappingProfiles
{
    public class DrinkMappingProfile : Profile
    {
        public DrinkMappingProfile()
        {
            CreateMap<DrinkDto, Drink>()
                .ForMember(x => x.Id, options => options.MapFrom(x => Guid.NewGuid()))
                .ForMember(x => x.ImageId, options => options.Ignore());
        }
    }
}
