using ParkingHere.Application.Abstractions;
using ParkingHere.Domain.Spots.Repositories;
using ParkingHere.Domain.Spots.Entities;
using ParkingHere.Application.Common.Exceptions;

namespace ParkingHere.Application.Spots.Commands.Handlers
{
    public class UpdateSpotHandler : ICommandHandler<UpdateSpot>
    {
        private readonly ISpotRepository _spotRepository;

        public UpdateSpotHandler(ISpotRepository spotRepository)
        {
            _spotRepository = spotRepository;
        }

        public async Task HandleAsync(UpdateSpot command)
        {
            var spot = await _spotRepository.GetByIdAsync(command.SpotId);

            if (spot is null)
            {
                throw new NotFoundException("Spot not found");
            }

            var newSpot = new Spot
            {
                Id = command.SpotId,
                Price = command.Price,
                Type = command.Type,
            };

            await _spotRepository.UpdateAsync(newSpot);
        }
    }
}
