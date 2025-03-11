using ParkingHere.Domain.Parkings.Entities;

namespace ParkingHere.Domain.Parkings.Repositories
{
    public interface IParkingRepository
    {
        Task<IEnumerable<Parking>> GetAllAsync();
        Task<Parking> GetByIdAsync(Guid id);
        Task AddAsync(Parking parking);
        Task UpdateAsync(Parking parking);
        Task DeleteAsync(Parking parking);
        Task<IEnumerable<Parking>> GetParkingsByUserIdAsync(Guid userId);
        Task<IEnumerable<Parking>> GetParkingsByParamsAsync(string city, DateTime startDate, DateTime endDate);
    }
}