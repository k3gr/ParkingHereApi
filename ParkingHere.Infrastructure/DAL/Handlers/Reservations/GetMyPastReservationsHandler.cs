using Microsoft.EntityFrameworkCore;
using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Reservations.DTO;
using ParkingHere.Application.Reservations.Queries;

namespace ParkingHere.Infrastructure.DAL.Handlers.Reservations
{
    public class GetMyPastReservationsHandler : IQueryHandler<GetMyPastReservations, IEnumerable<ReservationDto>>
    {
        private readonly ParkingDbContext _dbContext;

        public GetMyPastReservationsHandler(ParkingDbContext dbContext)
        => _dbContext = dbContext;

        public async Task<IEnumerable<ReservationDto>> HandleAsync(GetMyPastReservations query)
        {
            var reservations = await _dbContext.Reservations
                .Include(r => r.Parking.Address)
                .Include(r => r.Vehicle)
                .Where(r => r.CreatedById == query.UserId)
                .Where(r => r.EndDate < DateTime.UtcNow)
                .Select(r => r.AsDto())
                .ToListAsync();

            return reservations;
        }
    }
}
