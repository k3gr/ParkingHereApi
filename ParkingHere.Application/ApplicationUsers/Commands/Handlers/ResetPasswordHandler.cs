using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Common.Exceptions;
using ParkingHere.Application.Security;
using ParkingHere.Domain.ApplicationUsers.Repositories;

namespace ParkingHere.Application.ApplicationUsers.Commands.Handlers
{
    public class ResetPasswordHandler : ICommandHandler<ResetPassword>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordManager _passwordManager;

        public ResetPasswordHandler(IUserRepository userRepository, IPasswordManager passwordManager)
        {
            _userRepository = userRepository;
            _passwordManager = passwordManager;
        }

        public async Task HandleAsync(ResetPassword command)
        {
            var user = await _userRepository.GetByPasswordResetTokenAsync(command.Token);

            if (user == null || user.ResetTokenExpires < DateTime.Now)
            {
                throw new BadRequestException("Invalid token");
            }
            var hashedPassword = _passwordManager.Secure(command.Password);

            user.PasswordHash = hashedPassword;
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            await _userRepository.UpdateAsync(user);
        }
    }
}
