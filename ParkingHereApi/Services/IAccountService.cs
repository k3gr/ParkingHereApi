using ParkingHereApi.Entities;
using ParkingHereApi.Models;

namespace ParkingHereApi.Services
{
    public interface IAccountService
    {
        int RegisterUser(RegisterUserDto dto);
        void Update(int id, UpdateUserDto dto);
        UserDto GetById(int id);
        UserTokenDto GenerateJwt(LoginDto dto);
    }
}
