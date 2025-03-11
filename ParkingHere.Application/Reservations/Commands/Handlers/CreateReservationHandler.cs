using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Common.Exceptions;
using ParkingHere.Domain.Parkings;
using ParkingHere.Domain.Parkings.Repositories;
using ParkingHere.Domain.Reservations.Entities;
using ParkingHere.Domain.Reservations.Repositories;
using ParkingHere.Domain.Spots.Services;
using ParkingHere.Domain.Vehicles.Repositories;

namespace ParkingHere.Application.Reservations.Commands.Handlers
{
    public class CreateReservationHandler : ICommandHandler<CreateReservation>
    {
        private readonly IReservationRepository _repository;
        private readonly IParkingRepository _parkingRepository;
        private readonly IVehicleRepository _userRepository;
        private readonly ISpotService _spotService;

        public CreateReservationHandler(IReservationRepository repository,
            IVehicleRepository userRepository, IParkingRepository parkingRepository,
            ISpotService spotService)
        {
            _repository = repository;
            _parkingRepository = parkingRepository;
            _userRepository = userRepository;
            _spotService = spotService;
        }

        public async Task HandleAsync(CreateReservation command)
        {
            var parking = await _parkingRepository.GetByIdAsync(command.ParkingId);

            if (parking is null)
            {
                throw new ParkingNotFoundException();
            }

            var spotId = _spotService.GetFirstAvailableSpotByType(parking, command.StartDate, command.EndDate, command.Type);
            var vehicle = await _userRepository.GetByUserAsync(command.UserId);

            if (vehicle is null)
            {
                throw new NotFoundException("Vehicle not found");
            }

            if (command.StartDate < DateTime.Today)
            {
                throw new BadRequestException("Start date cannot be past");
            }

            var reservation = new Reservation
            {
                ParkingId = command.ParkingId,
                SpotId = spotId,
                Vehicle = vehicle,
                CreatedById = command.UserId,
                StartDate = command.StartDate,
                EndDate = command.EndDate,
            };

            await _repository.AddAsync(reservation);
        }
    }
}