using Microsoft.AspNetCore.Http;
using ParkingHere.Application.ApplicationUsers.DTO;
using ParkingHere.Application.Security;

namespace ParkingHere.Infrastructure.Auth
{
    internal sealed class HttpContextTokenStorage : ITokenStorage
    {
        private const string TokenKey = "jwt";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextTokenStorage(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Set(JwtDto jwt) => _httpContextAccessor.HttpContext?.Items.TryAdd(TokenKey, jwt);

        public JwtDto Get()
        {
            if (_httpContextAccessor.HttpContext is null)
            {
                return null;
            }

            if (_httpContextAccessor.HttpContext.Items.TryGetValue(TokenKey, out var jwt))
            {
                return jwt as JwtDto;
            }

            return null;
        }
    }
}
