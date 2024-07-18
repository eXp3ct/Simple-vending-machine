using AutoMapper;
using Expect.Vending.Domain.Dtos;
using Expect.Vending.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.Vending.Infrastructure.MappingProfiles
{
    public class ImageMappingProfile : Profile
    {
        public ImageMappingProfile()
        {
            CreateMap<ImageDto, Image>()
                .ForMember(x => x.Id, options => options.MapFrom(x => Guid.NewGuid()));
        }
    }
}
