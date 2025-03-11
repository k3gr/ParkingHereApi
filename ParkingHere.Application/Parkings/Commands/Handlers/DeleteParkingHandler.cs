using ParkingHere.Application.Abstractions;
using ParkingHere.Domain.Parkings.Repositories;

namespace ParkingHere.Application.Parkings.Commands.Handlers
{
    public class DeleteParkingHandler : ICommandHandler<DeleteParking>
    {
        private readonly IParkingRepository _parkingRepository;

        public DeleteParkingHandler(IParkingRepository parkingRepository)
        {
            _parkingRepository = parkingRepository;
        }

        public async Task HandleAsync(DeleteParking command)
        {
            var parking = await _parkingRepository.GetByIdAsync(command.ParkingId);
            if (parking != null)
            {
                await _parkingRepository.DeleteAsync(parking);
            }
        }
    }
}
