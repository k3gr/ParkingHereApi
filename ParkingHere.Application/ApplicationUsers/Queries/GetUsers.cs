using ParkingHere.Application.Abstractions;
using ParkingHere.Application.ApplicationUsers.DTO;

namespace ParkingHere.Application.ApplicationUsers.Queries
{
    public class GetUsers : IQuery<IEnumerable<UserDto>>
    {
    }
}
