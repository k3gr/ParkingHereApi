using ParkingHere.Application.Abstractions;

namespace ParkingHere.Application.Parkings.Commands;
public record CreateParking(Guid ParkingId, Guid UserId, string Name, string Description, string Type, string ContactEmail, string ContactNumber, string City, string Street, string PostalCode) : ICommand;
