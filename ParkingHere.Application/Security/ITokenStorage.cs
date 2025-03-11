using ParkingHere.Application.ApplicationUsers.DTO;

namespace ParkingHere.Application.Security
{
    public interface ITokenStorage
    {
        void Set(JwtDto jwt);
        JwtDto Get();
    }
}
