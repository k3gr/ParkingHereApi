using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ParkingHere.Application.Security;
using ParkingHere.Domain.ApplicationUsers.Entities;

namespace ParkingHere.Infrastructure.Security
{
    internal static class Extensions
    {
        public static IServiceCollection AddSecurity(this IServiceCollection services)
        {
            services
                .AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>()
                .AddSingleton<IPasswordManager, PasswordManager>();

            return services;
        }
    }
}
