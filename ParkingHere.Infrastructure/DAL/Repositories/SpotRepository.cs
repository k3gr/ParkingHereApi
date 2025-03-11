using Microsoft.EntityFrameworkCore;
using ParkingHere.Domain.Spots.Entities;
using ParkingHere.Domain.Spots.Repositories;
using ParkingHere.Infrastructure.DAL;

namespace ParkingHere.Infrastucture.DAL.Repositories
{
    public class SpotRepository : ISpotRepository
    {
        private readonly DbSet<Spot> _spots;

        public SpotRepository(ParkingDbContext dbContext)
        {
            _spots = dbContext.Spots;
        }

        public async Task<Spot> GetByIdAsync(Guid id)
        => await _spots
            .Include(x => x.Reservations)
            .SingleOrDefaultAsync(x => x.Id == id);

        public async Task AddAsync(Spot spot)
        => await _spots.AddAsync(spot);

        public async Task<IEnumerable<Spot>> GetAllAsync()
        => await _spots
            .Include(x => x.Reservations)
            .ToListAsync();

        public Task UpdateAsync(Spot spot)
        {
            _spots.Update(spot);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Spot>> GetAvailableSpotsByParamsAsync(Guid parkingId, DateTime startDate, DateTime endDate)
        {
            return await _spots
            .Include(s => s.Reservations)
            .Where(s => s.ParkingId == parkingId && s.Reservations.All(r => r.EndDate < startDate || r.StartDate > endDate))
            .ToListAsync();
        }
    }
}
