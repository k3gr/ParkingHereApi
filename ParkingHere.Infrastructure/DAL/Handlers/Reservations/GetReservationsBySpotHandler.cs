using Microsoft.EntityFrameworkCore;
using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Reservations.DTO;
using ParkingHere.Application.Reservations.Queries;

namespace ParkingHere.Infrastructure.DAL.Handlers.Reservations;
public class GetReservationsBySpotHandler : IQueryHandler<GetReservationsBySpot, IEnumerable<ReservationDto>>
{
    private readonly ParkingDbContext _dbContext;

    public GetReservationsBySpotHandler(ParkingDbContext dbContext)
    => _dbContext = dbContext;

    public async Task<IEnumerable<ReservationDto>> HandleAsync(GetReservationsBySpot query)
    {
        var reservations = await _dbContext.Reservations
            .Where(r => r.Id == query.SpotId)
            .Select(r => r.AsDto())
            .ToListAsync();

        return reservations;
    }
}
