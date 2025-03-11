using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Common.Exceptions;
using ParkingHere.Domain.ApplicationUsers.Entities;
using ParkingHere.Domain.ApplicationUsers.Repositories;
using ParkingHere.Domain.Parkings.Entities;
using ParkingHere.Domain.Parkings.Repositories;

namespace ParkingHere.Application.Parkings.Commands.Handlers
{
    public class CreateParkingHandler : ICommandHandler<CreateParking>

    {
        private readonly IParkingRepository _repository;
        private readonly IUserRepository _userRepository;

        public CreateParkingHandler(IParkingRepository repository,
            IUserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task HandleAsync(CreateParking command)
        {
            var user = await _userRepository.GetByIdAsync(command.UserId);
            if (user == null)
            {
                throw new BadRequestException("User not found");
            }

            var parking = new Parking
            {
                Name = command.Name,
                Description = command.Description,
                Type = command.Type,
                ContactEmail = command.ContactEmail,
                ContactNumber = command.ContactNumber,
                CreatedBy = user,
                Address = new Address(command.City, command.Street, command.PostalCode)
            };

            await _repository.AddAsync(parking);
        }
    }
}
