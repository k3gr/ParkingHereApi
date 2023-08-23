using ParkingHereApi.Entities;
using ParkingHereApi.Models;

namespace ParkingHereApi.Services
{
    public interface IAccountService
    {
        int RegisterUser(RegisterUserDto dto);
        void Update(int id, UpdateUserDto dto);
        UserDto GetById(int id);
        UserTokenDto Login(LoginDto dto);
        void Activation(string token);
        void VerifyPasswordResetToken(string token);
        void ForgotPassword(UserResetPasswordStep1Dto userResetPasswordStep1Dto);
        void ResetPassword(string token, UserResetPasswordStep2Dto userResetPasswordDto);
    }
}
