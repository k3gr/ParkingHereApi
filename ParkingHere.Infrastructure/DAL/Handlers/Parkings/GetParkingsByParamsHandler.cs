using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Parkings.DTO;
using ParkingHere.Application.Parkings.Queries;
using ParkingHere.Domain.Parkings.Repositories;

namespace ParkingHere.Infrastructure.DAL.Handlers.Parkings
{
    public class GetParkingsByParamsHandler : IQueryHandler<GetParkingsByParams, IEnumerable<ParkingDto>>
    {
        private readonly IParkingRepository _parkingRepository;

        public GetParkingsByParamsHandler(IParkingRepository parkingRepository)
        {
            _parkingRepository = parkingRepository;
        }

        public async Task<IEnumerable<ParkingDto>> HandleAsync(GetParkingsByParams query)
        {
            var parkings = await _parkingRepository.GetParkingsByParamsAsync(query.City, query.StartDate, query.EndDate);
            return parkings.Select(p => p.AsDto());
        }
    }
}
