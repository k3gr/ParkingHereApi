using ParkingHere.Application.Abstractions;
using ParkingHere.Domain.Parkings;
using ParkingHere.Domain.Parkings.Repositories;
using ParkingHere.Domain.Reservations.Entities;
using ParkingHere.Domain.Reservations.Repositories;
using ParkingHere.Domain.Spots.Services;

namespace ParkingHere.Application.Reservations.Commands.Handlers
{
    public class UpdateReservationHandler : ICommandHandler<UpdateReservation>
    {
        private readonly IReservationRepository _repository;
        private readonly IParkingRepository _parkingRepository;
        private readonly ISpotService _spotService;

        public UpdateReservationHandler(IReservationRepository repository,
            IParkingRepository parkingRepository, ISpotService spotService)
        {
            _repository = repository;
            _parkingRepository = parkingRepository;
            _spotService = spotService;
        }

        public async Task HandleAsync(UpdateReservation command)
        {
            var parking = await _parkingRepository.GetByIdAsync(command.ParkingId);

            if (parking is null)
            {
                throw new ParkingNotFoundException();
            }

            var spotId = _spotService.GetFirstAvailableSpotByType(parking, command.StartDate, command.EndDate, command.Type);

            var reservation = new Reservation
            {
                ParkingId = command.ParkingId,
                SpotId = spotId,
                //VehicleId = command.VehicleId,
                CreatedById = command.UserId
            };

            await _repository.UpdateAsync(reservation);
        }
    }
}