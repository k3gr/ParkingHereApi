using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Common.Exceptions;
using ParkingHere.Domain.ApplicationUsers.Repositories;

namespace ParkingHere.Application.ApplicationUsers.Commands.Handlers
{
    public class UpdateUserHandler : ICommandHandler<UpdateUser>
    {
        private readonly IUserRepository _repository;
        public UpdateUserHandler(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(UpdateUser command)
        {
            var user = await _repository.GetByIdAsync(command.UserId);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            user.FirstName = command.FirstName;
            user.LastName = command.LastName;
            user.Email = command.Email;

            await _repository.UpdateAsync(user);
        }
    }
}
