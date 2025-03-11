using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Common.Exceptions;
using ParkingHere.Domain.Parkings.Repositories;

namespace ParkingHere.Application.Parkings.Commands.Handlers
{
    public class UpdateParkingHandler : ICommandHandler<UpdateParking>
    {
        private readonly IParkingRepository _repository;

        public UpdateParkingHandler(IParkingRepository repository)
        {
            _repository = repository;
        }
        public async Task HandleAsync(UpdateParking command)
        {
            var parking = await _repository.GetByIdAsync(command.ParkingId);

            if (parking is null)
            {
                throw new NotFoundException("User not found");
            }

            parking.Name = command.Name;
            parking.Address.Street = command.Street;
            parking.Address.City = command.City;
            parking.Address.PostalCode = command.PostalCode;
            parking.Description = command.Description;
            parking.Type = command.Type;
            parking.ContactEmail = command.ContactEmail;
            parking.ContactNumber = command.ContactNumber;

            await _repository.UpdateAsync(parking);
        }
    }
}
