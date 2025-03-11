using ParkingHere.Application.ApplicationUsers.DTO;
using ParkingHere.Application.Parkings.DTO;
using ParkingHere.Application.Reservations.DTO;
using ParkingHere.Application.Spots.DTO;
using ParkingHere.Application.Vehicles.DTO;
using ParkingHere.Domain.ApplicationUsers.Entities;
using ParkingHere.Domain.Parkings.Entities;
using ParkingHere.Domain.Reservations.Entities;
using ParkingHere.Domain.Spots.Entities;
using ParkingHere.Domain.Vehicles.Entities;

namespace ParkingHere.Infrastructure.DAL.Handlers;
public static class Extensions
{
    public static ParkingDto AsDto(this Parking entity)
    => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Description = entity.Description,
        Type = entity.Type,
        ContactEmail = entity.ContactEmail,
        ContactNumber = entity.ContactNumber,
        City = entity.Address?.City,
        Street = entity.Address?.Street,
        PostalCode = entity.Address?.PostalCode,
        Prices = entity.Spots?.Select(s => s.Price).ToList(),
        Spots = entity.Spots?.Select(s => s.AsDto()).ToList(),
        Reservations = entity.Reservations?.Select(r => r.AsDto()).ToList() ?? new List<ReservationDto>()
    };

    public static UserDto AsDto(this User entity)
    => new()
    {
        Id = entity.Id,
        FirstName = entity.FirstName,
        LastName = entity.LastName,
        Email = entity.Email,
        RoleName = entity.Role.Name,
        RegistrationPlate = entity.Vehicle?.RegistrationPlate,
        VehicleId = entity.VehicleId ?? Guid.Empty
    };

    public static SpotDto AsDto(this Spot entity)
    => new()
    {
        Id = entity.Id,
        Price = entity.Price,
        Type = entity.Type,
        IsAvailable = entity.IsAvailable,
        Reservations = entity.Reservations?.Select(r => r.AsDto()).ToList()
    };

    public static VehicleDto AsDto(this Vehicle entity)
    => new()
    {
        Id = entity.Id,
        Brand = entity.Brand,
        Model = entity.Model,
        RegistrationPlate = entity.RegistrationPlate,
        CreatedById = entity.CreatedById
    };

    public static ReservationDto AsDto(this Reservation entity)
    => new()
    {
        Id = entity.Id,
        StartDate = entity.StartDate,
        EndDate = entity.EndDate,
        ParkingAddress = $"{entity.Parking?.Name} {entity.Parking?.Address?.Street} {entity.Parking?.Address?.City} {entity.Parking?.Address?.PostalCode}",
        VehicleDetails = $"{entity.Vehicle?.Brand} {entity.Vehicle?.Model} {entity.Vehicle?.RegistrationPlate}",

    };
}
