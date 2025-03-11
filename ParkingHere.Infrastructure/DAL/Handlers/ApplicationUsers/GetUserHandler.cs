using Microsoft.EntityFrameworkCore;
using ParkingHere.Application.Abstractions;
using ParkingHere.Application.ApplicationUsers.DTO;
using ParkingHere.Application.ApplicationUsers.Queries;

namespace ParkingHere.Infrastructure.DAL.Handlers.ApplicationUsers
{
    public sealed class GetUserHandler : IQueryHandler<GetUser, UserDto>
    {
        private readonly ParkingDbContext _dbContext;

        public GetUserHandler(ParkingDbContext dbContext)
            => _dbContext = dbContext;

        public async Task<UserDto> HandleAsync(GetUser query)
        {
            var user = await _dbContext.Users
                .Include(s => s.Role)
                .Include(v => v.Vehicle)
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == query.UserId);

            return user?.AsDto();
        }
    }
}
