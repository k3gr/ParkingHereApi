using ParkingHere.Application.Abstractions;

namespace ParkingHere.Application.Reservations.Commands;
public record UpdateReservation(Guid ParkingId, Guid UserId, Guid VehicleId, DateTime StartDate, DateTime EndDate, string Type) : ICommand;
