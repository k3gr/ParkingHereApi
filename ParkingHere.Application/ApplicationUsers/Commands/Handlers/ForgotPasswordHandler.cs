using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Common.Exceptions;
using ParkingHere.Domain.ApplicationUsers.Repositories;

namespace ParkingHere.Application.ApplicationUsers.Commands.Handlers
{
    public class ForgotPasswordHandler : ICommandHandler<ForgotPassword>
    {
        private readonly IUserRepository _userRepository;

        public ForgotPasswordHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task HandleAsync(ForgotPassword command)
        {
            var user = await _userRepository
                .GetByEmailAsync(command.Email);

            if (user == null)
            {
                throw new BadRequestException("User not found");
            }

            if (user.ActivationDate == null)
            {
                throw new ForbidException();
            }

            user.PasswordResetToken = Guid.NewGuid().ToString();
            user.ResetTokenExpires = DateTime.Now.AddDays(1);

            //await _emailService.SendPasswordResetEmail(user.Email, user.PasswordResetToken);
        }
    }
}
