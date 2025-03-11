using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Common.Exceptions;
using ParkingHere.Domain.Vehicles.Repositories;

namespace ParkingHere.Application.Vehicles.Commands.Handlers
{
    public class UpdateVehicleHandler : ICommandHandler<UpdateVehicle>
    {
        private readonly IVehicleRepository _repository;
        public UpdateVehicleHandler(IVehicleRepository repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(UpdateVehicle command)
        {
            var vehicle = await _repository.GetByUserAsync(command.UserId);

            if (vehicle is null)
            {
                throw new NotFoundException("Vehicle not found");
            }

            vehicle.Brand = command.Brand;
            vehicle.Model = command.Model;
            vehicle.RegistrationPlate = command.RegistrationPlate;

            await _repository.UpdateAsync(vehicle);
        }
    }
}
