using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ParkingHere.Application.ApplicationUsers.DTO;
using ParkingHere.Application.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ParkingHere.Infrastructure.Auth
{
    internal sealed class Authenticator : IAuthenticator
    {
        private readonly string _issuer;
        private readonly TimeSpan _expiry;
        private readonly string _audience;
        private readonly SigningCredentials _signingCredentials;
        private readonly JwtSecurityTokenHandler _jwtSecurityToken = new JwtSecurityTokenHandler();

        public Authenticator(IOptions<AuthOptions> options)
        {
            _issuer = options.Value.Issuer;
            _audience = options.Value.Audience;
            _expiry = options.Value.Expiry ?? TimeSpan.FromHours(3);
            _signingCredentials = new SigningCredentials(new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(options.Value.SigningKey)),
                    SecurityAlgorithms.HmacSha256);
        }

        public JwtDto CreateToken(Guid userId, string role)
        {
            var now = DateTime.UtcNow;
            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, userId.ToString()),
            new(ClaimTypes.Role, role)
        };

            var expires = now.Add(_expiry);
            var jwt = new JwtSecurityToken(_issuer, _audience, claims, now, expires, _signingCredentials);
            var token = _jwtSecurityToken.WriteToken(jwt);

            return new JwtDto
            {
                AccessToken = token,
                Expires = expires
            };
        }
    }
}
