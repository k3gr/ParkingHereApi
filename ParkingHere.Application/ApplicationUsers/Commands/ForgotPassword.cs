using ParkingHere.Application.Abstractions;

namespace ParkingHere.Application.ApplicationUsers.Commands
{
    public record ForgotPassword(string Email) : ICommand;
}
