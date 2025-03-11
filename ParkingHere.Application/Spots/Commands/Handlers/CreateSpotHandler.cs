using ParkingHere.Application.Abstractions;
using ParkingHere.Domain.Spots.Entities;
using ParkingHere.Domain.Spots.Repositories;
using ParkingHere.Domain.Spots.Services;

namespace ParkingHere.Application.Spots.Commands.Handlers
{
    public class CreateSpotHandler : ICommandHandler<CreateSpot>
    {
        private readonly ISpotRepository _repository;

        public CreateSpotHandler(ISpotRepository repository)
        {
            _repository = repository;
        }

        public async Task HandleAsync(CreateSpot command)
        {
            var spot = new Spot
            {
                Price = command.Price,
                Type = command.Type,
                ParkingId = command.ParkingId,
                IsAvailable = true
            };

            await _repository.AddAsync(spot);
        }
    }
}