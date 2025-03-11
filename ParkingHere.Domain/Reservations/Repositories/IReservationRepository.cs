using ParkingHere.Domain.Parkings.Entities;
using ParkingHere.Domain.Reservations.Entities;

namespace ParkingHere.Domain.Reservations.Repositories
{
    public interface IReservationRepository
    {
        Task<Reservation> GetByIdAsync(Guid id);
        Task AddAsync(Reservation reservation);
        Task UpdateAsync(Reservation reservation);
        Task DeleteAsync(Reservation reservation);
    }
}