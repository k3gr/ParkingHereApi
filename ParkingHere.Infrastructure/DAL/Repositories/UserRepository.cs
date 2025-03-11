using Microsoft.EntityFrameworkCore;
using ParkingHere.Domain.ApplicationUsers.Entities;
using ParkingHere.Domain.ApplicationUsers.Repositories;

namespace ParkingHere.Infrastructure.DAL.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly DbSet<User> _users;

        public UserRepository(ParkingDbContext dbContext)
        {
            _users = dbContext.Users;
        }

        public async Task AddAsync(User user)
        {
            await _users.AddAsync(user);
        }
        public Task UpdateAsync(User user)
        {
            _users.Update(user);
            return Task.CompletedTask;
        }

        public Task<User> GetByIdAsync(Guid id)
            => _users
            .Include(u => u.Vehicle)
            .SingleOrDefaultAsync(x => x.Id == id);

        public Task<User> GetByEmailAsync(string email)
            => _users.Include(r => r.Role).SingleOrDefaultAsync(x => x.Email == email);

        public Task<User> GetByActivationTokenAsync(string activationToken)
            => _users.SingleOrDefaultAsync(x => x.ActivationToken == activationToken);

        public Task<User> GetByPasswordResetTokenAsync(string token)
            => _users.SingleOrDefaultAsync(x => x.PasswordResetToken == token);
    }
}
