using ParkingHere.Application.Abstractions;

namespace ParkingHere.Application.Reservations.Commands;
public record CreateReservation(Guid ReservationId, Guid SpotId, Guid ParkingId, Guid UserId, Guid VehicleId, DateTime StartDate, DateTime EndDate, string Type) : ICommand;
