using ParkingHere.Domain.Spots.Entities;

namespace ParkingHere.Domain.Spots.Repositories
{
    public interface ISpotRepository
    {
        Task<IEnumerable<Spot>> GetAllAsync();
        Task<IEnumerable<Spot>> GetAvailableSpotsByParamsAsync(Guid parkingId, DateTime startDate, DateTime endDate);
        Task<Spot> GetByIdAsync(Guid id);
        Task AddAsync(Spot spot);
        Task UpdateAsync(Spot spot);
    }
}