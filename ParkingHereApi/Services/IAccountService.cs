using ParkingHereApi.Models;

namespace ParkingHereApi.Services
{
    public interface IAccountService
    {
        int RegisterUser(RegisterUserDto dto);
        UserDto GetById(int id);
        ClientTokenDto GenerateJwt(LoginDto dto);
    }
}
