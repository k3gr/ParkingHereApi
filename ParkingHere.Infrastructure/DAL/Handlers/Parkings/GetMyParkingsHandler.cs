using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Parkings.DTO;
using ParkingHere.Application.Parkings.Queries;
using ParkingHere.Domain.Parkings.Repositories;

namespace ParkingHere.Infrastructure.DAL.Handlers.Parkings
{
    public class GetMyParkingsHandler : IQueryHandler<GetMyParkings, IEnumerable<ParkingDto>>
    {
        private readonly IParkingRepository _parkingRepository;

        public GetMyParkingsHandler(IParkingRepository parkingRepository)
        {
            _parkingRepository = parkingRepository;
        }

        public async Task<IEnumerable<ParkingDto>> HandleAsync(GetMyParkings query)
        {
            var parkings = await _parkingRepository.GetParkingsByUserIdAsync(query.UserId);

            return parkings.Select(p => p.AsDto());
        }
    }
}
