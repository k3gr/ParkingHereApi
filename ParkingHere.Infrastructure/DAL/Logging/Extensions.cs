using Microsoft.Extensions.DependencyInjection;
using ParkingHere.Application.Abstractions;
using ParkingHere.Infrastructure.DAL.Logging.Decorators;

namespace ParkingHere.Infrastructure.DAL.Logging
{
    internal static class Extensions
    {
        public static IServiceCollection AddCustomLogging(this IServiceCollection services)
        {
            services.TryDecorate(typeof(ICommandHandler<>), typeof(LoggingCommandHandlerDecorator<>));

            return services;
        }
    }
}
