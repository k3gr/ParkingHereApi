using ParkingHere.Application.Abstractions;

namespace ParkingHere.Application.ApplicationUsers.Commands
{
    public record UpdateUser(Guid UserId, string FirstName, string LastName, string Email) :ICommand;
}
