using ParkingHere.Application.Abstractions;

namespace ParkingHere.Application.Spots.Commands
{
    public record UpdateSpot(Guid SpotId, Decimal Price, string Type) : ICommand;
}
