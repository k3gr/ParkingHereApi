using Microsoft.EntityFrameworkCore;
using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Spots.DTO;
using ParkingHere.Application.Spots.Queries;

namespace ParkingHere.Infrastructure.DAL.Handlers.Spots
{
    public sealed class GetSpotHandler : IQueryHandler<GetSpot, SpotDto>
    {
        private readonly ParkingDbContext _dbContext;

        public GetSpotHandler(ParkingDbContext dbContext)
            => _dbContext = dbContext;

        public async Task<SpotDto> HandleAsync(GetSpot query)
        {
            var spot = await _dbContext.Spots
                .Include(r => r.Reservations)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == query.SpotId);

            return spot?.AsDto();
        }
    }
}
