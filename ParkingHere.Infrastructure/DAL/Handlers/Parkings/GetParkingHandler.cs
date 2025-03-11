using Microsoft.EntityFrameworkCore;
using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Parkings.DTO;
using ParkingHere.Application.Parkings.Queries;

namespace ParkingHere.Infrastructure.DAL.Handlers.Parkings;
public sealed class GetParkingHandler : IQueryHandler<GetParking, ParkingDto>
{
    private readonly ParkingDbContext _dbContext;

    public GetParkingHandler(ParkingDbContext dbContext)
        => _dbContext = dbContext;

    public async Task<ParkingDto> HandleAsync(GetParking query)
    {
        var parking = await _dbContext.Parkings
            .Include(r => r.Reservations)
            .Include(s => s.Spots)
            .Include(a => a.Address)
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == query.ParkingId);

        return parking?.AsDto();
    }
}
