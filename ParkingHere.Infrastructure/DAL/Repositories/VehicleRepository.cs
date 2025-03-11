using Microsoft.EntityFrameworkCore;
using ParkingHere.Domain.Vehicles.Entities;
using ParkingHere.Domain.Vehicles.Repositories;

namespace ParkingHere.Infrastructure.DAL.Repositories
{
    internal class VehicleRepository : IVehicleRepository
    {
        private readonly DbSet<Vehicle> _vehicles;

        public VehicleRepository(ParkingDbContext dbContext)
        {
            _vehicles = dbContext.Vehicles;
        }

        public Task AddAsync(Vehicle vehicle)
        {
            _vehicles.AddAsync(vehicle);
            return Task.CompletedTask;
        }

        public Task<Vehicle> GetByUserAsync(Guid userId)
            => _vehicles.SingleOrDefaultAsync(x => x.CreatedById == userId);


        public Task UpdateAsync(Vehicle vehicle)
        {
            _vehicles.Update(vehicle);
            return Task.CompletedTask;
        }
    }
}
