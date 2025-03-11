using ParkingHere.Application.Abstractions;

namespace ParkingHere.Application.Spots.Commands
{
    public record CreateSpot(Guid SpotId, Guid ParkingId, decimal Price, string Type) : ICommand;
}
