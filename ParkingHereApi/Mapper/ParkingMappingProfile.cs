using AutoMapper;
using ParkingHereApi.Entities;
using ParkingHereApi.Models;

namespace ParkingHereApi.Mapper
{
    public class ParkingMappingProfile : Profile
    {
        public ParkingMappingProfile() 
        {
            CreateMap<Parking, ParkingDto>()
                .ForMember(m => m.City, c => c.MapFrom(s => s.Address.City))
                .ForMember(m => m.Street, c => c.MapFrom(s => s.Address.Street))
                .ForMember(m => m.PostalCode, c => c.MapFrom(s => s.Address.PostalCode));

            CreateMap<ParkingSpot, ParkingSpotDto>();

            CreateMap<CreateParkingDto, Parking>()
                .ForMember(p => p.Address,
                    c => c.MapFrom(dto => new Address()
                    { City = dto.City, PostalCode = dto.PostalCode, Street = dto.Street }));

            CreateMap<CreateParkingSpotDto, ParkingSpot>();
        }
    }
}
