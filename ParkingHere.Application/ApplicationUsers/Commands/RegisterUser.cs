using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Vehicles.DTO;

namespace ParkingHere.Application.ApplicationUsers.Commands;

public record RegisterUser(Guid UserId, Guid VehicleId, string FirstName, string LastName, string Email, string Password,
    string ConfirmPassword, string? ActivationToken, DateTime? DateOfBirth, Guid RoleId, VehicleDto Vehicle) : ICommand;
