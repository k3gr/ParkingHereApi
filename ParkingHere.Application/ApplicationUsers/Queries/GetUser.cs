using ParkingHere.Application.Abstractions;
using ParkingHere.Application.ApplicationUsers.DTO;

namespace ParkingHere.Application.ApplicationUsers.Queries
{
    public class GetUser : IQuery<UserDto>
    {
        public Guid UserId { get; set; }
    }
}
