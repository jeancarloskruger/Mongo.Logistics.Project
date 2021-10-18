using AutoMapper;
using Mongo.Logistics.Project.API.Dtos;
using Mongo.Logistics.Project.API.Entities;

namespace Mongo.Logistics.Project.API
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<CityDto, City>()
                .ForMember(dst => dst.Id, src => src.MapFrom(a => a.Name))
                .ForMember(dst => dst.Position, src => src.MapFrom(a => a.Location))
               .ReverseMap();

            CreateMap<PlaneDto, Plane>()
                .ForMember(dst => dst.Id, src => src.MapFrom(a => a.Callsign))
                .ReverseMap();
        }
    }
}
