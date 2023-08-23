using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ParkingHereApi.Authorization;
using ParkingHereApi.Common.Models;
using ParkingHereApi.Entities;
using ParkingHereApi.Enums;
using ParkingHereApi.Exceptions;
using ParkingHereApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
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
        private readonly IEmailService _emailService;

        public AccountService(ParkingDbContext dbContext, IMapper mapper, IPasswordHasher<User> passwordHasher,
            AuthenticationSettings authenticationSettings, IAuthorizationService authorizationService, IUserContextService userContextService, IEmailService emailService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
            _emailService = emailService;
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
                },
                ActivationToken = CreateRandomToken(),
            };
            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);

            newUser.PasswordHash = hashedPassword;
            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();

            newUser.Vehicle.CreatedById = newUser.Id;
            _dbContext.SaveChanges();

            var createEmail = new CreateEmail
            {
                EmailSubject = "Aktywuj konto",
                EmailTo = newUser.Email,
                EmailBody = $"Witaj {newUser.FirstName},\r\nkliknij w poniższy link aby aktywować konto: \r\nhttp://127.0.0.1:5173/activation?token={newUser.ActivationToken}",
                EmailToName = $"{newUser.FirstName} {newUser.LastName}"
            };

            if (!_emailService.SendMail(createEmail))
            {
                throw new BadRequestException("Email not delivered");
            }

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

        public UserTokenDto Login(LoginDto dto)
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

            if (user.ActivationDate == null)
            {
                throw new BadRequestException("User not verified");
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, $"{user.Role.Name}"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(_authenticationSettings.JwtExpireMinutes);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer,
                _authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();

            var userToken = new UserTokenDto { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email, Token = tokenHandler.WriteToken(token), Expires = expires };

            return userToken;
        }

        public void Activation(string token)
        {
            var user = _dbContext
                .Users
                .FirstOrDefault(u => u.ActivationToken == token);

            if (user == null || user.ActivationDate != null)
            {
                throw new BadRequestException("Invalid token");
            }

            user.ActivationDate = DateTime.Now;
            _dbContext.SaveChanges();
        }

        public void VerifyPasswordResetToken(string token)
        {
            var user = _dbContext
                .Users
                .FirstOrDefault(u => u.PasswordResetToken == token);

            if (user == null)
            {
                throw new BadRequestException("Invalid token");
            }

            _dbContext.SaveChanges();
        }

        public void ForgotPassword(UserResetPasswordStep1Dto userResetPasswordStep1Dto)
        {
            var user = _dbContext
                .Users
                .FirstOrDefault(u => u.Email == userResetPasswordStep1Dto.Email);

            if (user == null)
            {
                throw new BadRequestException("User not found");
            }

            user.PasswordResetToken = CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddDays(1);

            _dbContext.SaveChanges();

            var createEmail = new CreateEmail
            {
                EmailSubject = "Przywróć hasło",
                EmailTo = user.Email,
                EmailBody = $"Witaj {user.FirstName},\r\nkliknij w poniższy link aby przywrócić hasło: \r\nhttp://127.0.0.1:5173/reset-password?token={user.PasswordResetToken}",
                EmailToName = $"{user.FirstName} {user.LastName}"
            };

            if (!_emailService.SendMail(createEmail))
            {
                throw new BadRequestException("Email not delivered");
            }
        }

        public void ResetPassword(string token, UserResetPasswordStep2Dto userResetPasswordDto)
        {
            var user = _dbContext
                .Users
                .FirstOrDefault(u => u.PasswordResetToken == token);

            if (user == null || user.ResetTokenExpires < DateTime.Now)
            {
                throw new BadRequestException("Invalid token");
            }
            var hashedPassword = _passwordHasher.HashPassword(user, userResetPasswordDto.Password);

            user.PasswordHash = hashedPassword;
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            _dbContext.SaveChanges();
        }

        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }
    }
}
