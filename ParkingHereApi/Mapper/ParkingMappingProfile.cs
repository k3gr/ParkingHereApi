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

            CreateMap<Spot, SpotDto>();

            CreateMap<CreateParkingDto, Parking>()
                .ForMember(p => p.Address,
                    c => c.MapFrom(dto => new Address()
                    { City = dto.City, PostalCode = dto.PostalCode, Street = dto.Street }));

            CreateMap<CreateSpotDto, Spot>();
            CreateMap<Reservation, ReservationDto>();
            CreateMap<CreateReservationDto, Reservation>();
            CreateMap<User, UserDto>()
                .ForMember(u => u.VehicleBrand, c => c.MapFrom(v => v.Vehicle.Brand))
                .ForMember(u => u.VehicleModel, c => c.MapFrom(v => v.Vehicle.Model))
                .ForMember(u => u.VehicleRegistrationPlate, c => c.MapFrom(v => v.Vehicle.RegistrationPlate));
            CreateMap<Vehicle, VehicleDto>();
            CreateMap<VehicleDto, Vehicle>();
        }
    }
}
