using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Spots.DTO;
using ParkingHere.Application.Spots.Queries;
using ParkingHere.Domain.Spots.Repositories;

namespace ParkingHere.Infrastructure.DAL.Handlers.Spots
{
    public class GetAvailableSpotsByParamsHandler : IQueryHandler<GetAvailableSpotsByParams, IEnumerable<SpotDto>>
    {
        private readonly ISpotRepository _spotRepository;

        public GetAvailableSpotsByParamsHandler(ISpotRepository spotRepository)
        {
            _spotRepository = spotRepository;
        }

        public async Task<IEnumerable<SpotDto>> HandleAsync(GetAvailableSpotsByParams query)
        {
            var spots = await _spotRepository.GetAvailableSpotsByParamsAsync(query.ParkingId, query.StartDate, query.EndDate);

            return spots.Select(p => p.AsDto());
        }
    }
}
