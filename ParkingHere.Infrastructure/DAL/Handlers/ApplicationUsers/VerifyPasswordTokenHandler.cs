using ParkingHere.Application.Abstractions;
using ParkingHere.Application.ApplicationUsers.Queries;
using ParkingHere.Application.Common.Exceptions;

namespace ParkingHere.Infrastructure.DAL.Handlers.ApplicationUsers
{
    public class VerifyPasswordTokenHandler : IQueryHandler<VerifyPasswordToken, bool>
    {
        private readonly ParkingDbContext _dbContext;

        public VerifyPasswordTokenHandler(ParkingDbContext dbContext)
            => _dbContext = dbContext;

        public async Task<bool> HandleAsync(VerifyPasswordToken query)
        {
            var user = _dbContext
                .Users
                .FirstOrDefault(u => u.PasswordResetToken == query.Token);

            if (user == null)
            {
                throw new BadRequestException("Invalid token");
            }

            return true;
        }
    }
}
