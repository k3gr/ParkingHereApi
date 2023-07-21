using ParkingHereApi.Models;

namespace ParkingHereApi.Services
{
    public interface IAccountService
    {
        int RegisterUser(RegisterUserDto dto);
        ClientTokenDto GenerateJwt(LoginDto dto);
    }
}
