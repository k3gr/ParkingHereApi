using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ParkingHere.Application.Abstractions;
using ParkingHere.Domain.ApplicationUsers.Repositories;
using ParkingHere.Domain.Parkings.Repositories;
using ParkingHere.Domain.Reservations.Repositories;
using ParkingHere.Domain.Spots.Repositories;
using ParkingHere.Domain.Vehicles.Repositories;
using ParkingHere.Infrastructure.DAL.Decorators;
using ParkingHere.Infrastructure.DAL.Repositories;
using ParkingHere.Infrastructure.DAL.Seeder;
using ParkingHere.Infrastucture.DAL.Repositories;

namespace ParkingHere.Infrastructure.DAL
{
    internal static class Extensions
    {
        public static IServiceCollection AddMySQL(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ParkingDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("ParkingHereDbConnection")));
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<IParkingRepository, ParkingRepository>();
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ISpotRepository, SpotRepository>();
            services.AddHostedService<DatabaseInitializer>();
            services.AddScoped<IUnitOfWork, ParkingHereUnitOfWork>();
            services.AddScoped< ParkingHereSeeder>();

            services.TryDecorate(typeof(ICommandHandler<>), typeof(UnitOfWorkCommandHandlerDecorator<>));

            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var seeder = scope.ServiceProvider.GetRequiredService<ParkingHereSeeder>();
                seeder.Seed();
            }

            return services;
        }
    }
}
