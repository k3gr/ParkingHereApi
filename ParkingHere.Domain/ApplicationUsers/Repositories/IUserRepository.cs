using ParkingHere.Domain.ApplicationUsers.Entities;

namespace ParkingHere.Domain.ApplicationUsers.Repositories
{
    public interface IUserRepository
    {
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<User> GetByIdAsync(Guid id);
        Task<User> GetByEmailAsync(string email);
        Task<User> GetByActivationTokenAsync(string email);
        Task<User> GetByPasswordResetTokenAsync(string token);
    }
}
