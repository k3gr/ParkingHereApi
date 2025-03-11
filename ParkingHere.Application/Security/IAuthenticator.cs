using ParkingHere.Application.ApplicationUsers.DTO;

namespace ParkingHere.Application.Security;
public interface IAuthenticator
{
    JwtDto CreateToken(Guid userId, string role);
}
