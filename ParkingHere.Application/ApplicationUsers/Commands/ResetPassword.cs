using ParkingHere.Application.Abstractions;

namespace ParkingHere.Application.ApplicationUsers.Commands
{
    public record ResetPassword(string Token, string Password) : ICommand;
}
