using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Common.Exceptions;
using ParkingHere.Application.Security;
using ParkingHere.Domain.ApplicationUsers.Entities;
using ParkingHere.Domain.ApplicationUsers.Repositories;
using ParkingHere.Domain.Vehicles.Entities;
using ParkingHere.Domain.Vehicles.Repositories;

namespace ParkingHere.Application.ApplicationUsers.Commands.Handlers
{
    public class RegisterUserHandler : ICommandHandler<RegisterUser>
    {
        private readonly IUserRepository _userRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IPasswordManager _passwordManager;

        public RegisterUserHandler(IUserRepository userRepository, IVehicleRepository vehicleRepository,
            IPasswordManager passwordManager)
        {
            _userRepository = userRepository;
            _vehicleRepository = vehicleRepository;
            _passwordManager = passwordManager;
        }

        public async Task HandleAsync(RegisterUser command)
        {
            if (await _userRepository.GetByEmailAsync(command.Email) is not null)
            {
                throw new EmailAlreadyInUseException(command.Email);
            }

            var securedPassword = _passwordManager.Secure(command.Password);
            var roleId = Guid.Parse("123e4567-e89b-12d3-a456-426614174044");

            Vehicle? vehicle = null;
            if (command.Vehicle is not null)
            {
                vehicle = new Vehicle
                {
                    Id = command.Vehicle.Id,
                    Brand = command.Vehicle.Brand,
                    Model = command.Vehicle.Model,
                    RegistrationPlate = command.Vehicle.RegistrationPlate,
                    CreatedById = command.UserId
                };

                await _vehicleRepository.AddAsync(vehicle);
            }

            var user = new User
            {
                Id = command.UserId,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
                PasswordHash = securedPassword,
                RoleId = roleId,
                ActivationToken = command.ActivationToken,
                VehicleId = vehicle?.Id
            };

            await _userRepository.AddAsync(user);
        }
    }
}