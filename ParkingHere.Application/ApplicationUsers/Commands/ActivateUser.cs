using ParkingHere.Application.Abstractions;

namespace ParkingHere.Application.ApplicationUsers.Commands
{
    public record ActivateUser(string Token) : ICommand;
}
