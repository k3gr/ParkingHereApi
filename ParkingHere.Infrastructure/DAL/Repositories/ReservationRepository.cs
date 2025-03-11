using Microsoft.EntityFrameworkCore;
using ParkingHere.Domain.Reservations.Entities;
using ParkingHere.Domain.Reservations.Repositories;

namespace ParkingHere.Infrastructure.DAL.Repositories
{
    internal class ReservationRepository : IReservationRepository
    {
        private readonly ParkingDbContext _dbContext;
        private readonly DbSet<Reservation> _reservations;

        public ReservationRepository(ParkingDbContext dbContext)
        {
            _dbContext = dbContext;
            _reservations = dbContext.Reservations;
        }

        public async Task<Reservation> GetByIdAsync(Guid id)
        {
            return await _reservations
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task AddAsync(Reservation reservation)
        => await _reservations.AddAsync(reservation);

        public async Task<IEnumerable<Reservation>> GetAllAsync(Guid parkingId, Guid spotId)
        {
            return await _reservations
                .ToListAsync();
        }

        public Task UpdateAsync(Reservation reservation)
        {
            _reservations.Update(reservation);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Reservation reservation)
        {
            _reservations.Remove(reservation);
            return Task.CompletedTask;
        }
    }
}
