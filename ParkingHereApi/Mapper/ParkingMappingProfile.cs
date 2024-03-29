﻿using AutoMapper;
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

            CreateMap<Reservation, ReservationDto>()
                .ForMember(r => r.ParkingName, c => c.MapFrom(p => p.Parking.Name))
                .ForMember(r => r.VehicleDetails, c => c.MapFrom(v => $"{v.Vehicle.RegistrationPlate} {v.Vehicle.Brand} {v.Vehicle.Model}"))
                .ForMember(r => r.ParkingAddress, c => c.MapFrom(p => $"{p.Parking.Address.Street} {p.Parking.Address.PostalCode} {p.Parking.Address.City}"));

            CreateMap<CreateReservationDto, Reservation>();

            CreateMap<User, UserDto>()
                .ForMember(u => u.VehicleId, c => c.MapFrom(v => v.Vehicle.Id));

            CreateMap<Vehicle, VehicleDto>();

            CreateMap<VehicleDto, Vehicle>();
        }
    }
}
