using Microsoft.EntityFrameworkCore;
using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Parkings.DTO;
using ParkingHere.Application.Parkings.Queries;

namespace ParkingHere.Infrastructure.DAL.Handlers.Parkings;
public sealed class GetParkingsHandler : IQueryHandler<GetParkings, IEnumerable<ParkingDto>>
{
    private readonly ParkingDbContext _dbContext;

    public GetParkingsHandler(ParkingDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<IEnumerable<ParkingDto>> HandleAsync(GetParkings query)
        => await _dbContext.Parkings
            .Include(r => r.Reservations)
            .Include(s => s.Spots)
            .ThenInclude(s => s.Reservations)
            .Include(a => a.Address)
            .AsNoTracking()
            .Select(x => x.AsDto())
            .ToListAsync();
}