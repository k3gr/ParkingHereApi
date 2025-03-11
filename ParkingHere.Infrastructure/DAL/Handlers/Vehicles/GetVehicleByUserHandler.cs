using Microsoft.EntityFrameworkCore;
using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Vehicles.DTO;
using ParkingHere.Application.Vehicles.Queries;

namespace ParkingHere.Infrastructure.DAL.Handlers.Vehicles
{
    public sealed class GetVehicleByUserHandler : IQueryHandler<GetVehicleByUser, VehicleDto>
    {
        private readonly ParkingDbContext _dbContext;

        public GetVehicleByUserHandler(ParkingDbContext dbContext)
            => _dbContext = dbContext;

        public async Task<VehicleDto> HandleAsync(GetVehicleByUser query)
        {
            var vehicle = await _dbContext.Vehicles
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.CreatedById == query.UserId);

            return vehicle?.AsDto();
        }
    }
}
