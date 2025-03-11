using Microsoft.EntityFrameworkCore;
using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Reservations.DTO;
using ParkingHere.Application.Reservations.Queries;

namespace ParkingHere.Infrastructure.DAL.Handlers.Reservations
{
    public class GetMyCurrentReservationsHandler : IQueryHandler<GetMyCurrentReservations, IEnumerable<ReservationDto>>
    {
        private readonly ParkingDbContext _dbContext;

        public GetMyCurrentReservationsHandler(ParkingDbContext dbContext)
        => _dbContext = dbContext;

        public async Task<IEnumerable<ReservationDto>> HandleAsync(GetMyCurrentReservations query)
        {
            var reservations = await _dbContext.Reservations
                .Include(r => r.Parking.Address)
                .Include(r => r.Vehicle)
                .Where(r => r.CreatedById == query.UserId)
                .Where(r => r.EndDate.Date >= DateTime.UtcNow)
                .Select(r => r.AsDto())
                .ToListAsync();

            return reservations;
        }
    }
}
