using Microsoft.EntityFrameworkCore;
using ParkingHere.Domain.Parkings.Entities;
using ParkingHere.Domain.Parkings.Repositories;
using ParkingHere.Infrastructure.DAL;

namespace ParkingHere.Infrastucture.DAL.Repositories
{
    internal class ParkingRepository : IParkingRepository
    {
        private readonly DbSet<Parking> _parkings;

        public ParkingRepository(ParkingDbContext dbContext)
        {
            _parkings = dbContext.Parkings;
        }

        public async Task<Parking> GetByIdAsync(Guid id)
        {
            return await _parkings
                .Include(s => s.Spots)
                .Include(p => p.Reservations)
                .Include(a => a.Address)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Parking parking)
        => await _parkings.AddAsync(parking);

        public async Task<IEnumerable<Parking>> GetAllAsync()
        {
            return await _parkings
                .Include(p => p.Spots)
                .Include(p => p.Reservations)
                .Include(a => a.Address)
                .ToListAsync();
        }

        public Task UpdateAsync(Parking parking)
        {
            _parkings.Update(parking);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Parking parking)
        {
            _parkings.Remove(parking);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<Parking>> GetParkingsByUserIdAsync(Guid userId)
        {
            return await _parkings
                .Include(p => p.Spots)
                .Include(p => p.Reservations)
                .Include(a => a.Address)
                .Where(p => p.CreatedById == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Parking>> GetParkingsByParamsAsync(string city, DateTime startDate, DateTime endDate)
        {
            return await _parkings
                .Include(p => p.Spots)
                .Include(p => p.Reservations)
                .Include(a => a.Address)
                .Where(p => p.Address.City.StartsWith(city) && p.Spots.Any(s => s.Reservations.All(r => r.EndDate < startDate || r.StartDate > endDate)))
                .ToListAsync();
        }
    }
}
