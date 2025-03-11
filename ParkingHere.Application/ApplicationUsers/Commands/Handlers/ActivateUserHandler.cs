using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Common.Exceptions;
using ParkingHere.Domain.ApplicationUsers.Repositories;

namespace ParkingHere.Application.ApplicationUsers.Commands.Handlers
{
    public class ActivateUserHandler : ICommandHandler<ActivateUser>
    {
        private readonly IUserRepository _userRepository;

        public ActivateUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task HandleAsync(ActivateUser command)
        {
            var user = await _userRepository
                .GetByActivationTokenAsync(command.Token);

            if (user == null || user.ActivationDate != null)
            {
                throw new BadRequestException("Invalid token");
            }

            user.ActivationDate = DateTime.Now;

            await _userRepository.UpdateAsync(user);
        }
    }
}
