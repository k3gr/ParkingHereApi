using Microsoft.Extensions.DependencyInjection;
using ParkingHere.Domain.Reservations.Policies;
using ParkingHere.Domain.Spots.Services;

namespace ParkingHere.Domain
{
    public static class Extensions
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            services.AddSingleton<IReservationPolicies, ReservationPolicies>();
            services.AddSingleton<ISpotService, SpotService>();

            return services;
        }
    }
}
