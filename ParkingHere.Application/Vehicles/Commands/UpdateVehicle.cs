using ParkingHere.Application.Abstractions;

namespace ParkingHere.Application.Vehicles.Commands
{
    public record UpdateVehicle(Guid UserId, string Brand, string Model, string RegistrationPlate) : ICommand;

}
