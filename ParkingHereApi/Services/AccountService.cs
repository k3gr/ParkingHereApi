using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ParkingHereApi.Authorization;
using ParkingHereApi.Entities;
using ParkingHereApi.Enums;
using ParkingHereApi.Exceptions;
using ParkingHereApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace ParkingHereApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly ParkingDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public AccountService(ParkingDbContext dbContext, IMapper mapper, IPasswordHasher<User> passwordHasher,
            AuthenticationSettings authenticationSettings, IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }
        public int RegisterUser(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                RoleId = dto.RoleId,
                Vehicle = new Vehicle()
                {
                    Brand = dto.Vehicle.Brand,
                    Model = dto.Vehicle.Model,
                    RegistrationPlate = dto.Vehicle.RegistrationPlate
                }
            };
            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);

            newUser.PasswordHash = hashedPassword;
            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();

            return newUser.Id;
        }

        public UserDto GetById(int id)
        {
            var user = _dbContext
                .Users
                .Include(v => v.Vehicle)
                .FirstOrDefault(u => u.Id == id);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        public void Update(int id, UpdateUserDto dto)
        {
            var user = _dbContext
                .Users
                .FirstOrDefault(u => u.Id == id);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.Email = dto.Email;

            _dbContext.SaveChanges();
        }

        public UserTokenDto GenerateJwt(LoginDto dto)
        {
            var user = _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == dto.Email);

            if (user is null)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Invalid username or password");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();

            var userToken = new UserTokenDto { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email, Token = tokenHandler.WriteToken(token) };

            return userToken;
        }
    }
}
