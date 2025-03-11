using ParkingHere.Domain.Vehicles.Entities;

namespace ParkingHere.Domain.Vehicles.Repositories
{
    public interface IVehicleRepository
    {
        Task AddAsync(Vehicle vehicle);
        Task<Vehicle> GetByUserAsync(Guid id);
        Task UpdateAsync(Vehicle vehicle);
    }
}
