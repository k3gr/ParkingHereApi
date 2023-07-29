using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ParkingHereApi.Entities;
using ParkingHereApi.Exceptions;
using ParkingHereApi.Models;

namespace ParkingHereApi.Services
{
    public class VehicleService :IVehicleService
    {
        private readonly ParkingDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public VehicleService(ParkingDbContext dbContext, IMapper mapper, IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        public VehicleDto GetById(int id)
        {
            var user = _dbContext
                .Users
                .FirstOrDefault(u => u.Id == id);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }
            var vehicle = _dbContext
                .Vehicles
                .FirstOrDefault(u => u.Id == user.VehicleId);

            if (vehicle is null)
            {
                throw new NotFoundException("Vehicle not found");
            }

            var vehicleDto = _mapper.Map<VehicleDto>(vehicle);

            return vehicleDto;
        }

        public void Update(int id, VehicleDto dto)
        {
            var user = _dbContext
                .Vehicles
                .FirstOrDefault(u => u.Id == id);

            if (user is null)
            {
                throw new NotFoundException("User not found");
            }

            //var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, user,
            //     new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            //if (!authorizationResult.Succeeded)
            //{
            //    throw new ForbidException();
            //}

            user.Brand = dto.Brand;
            user.Model = dto.Model;
            user.RegistrationPlate = dto.RegistrationPlate;

            _dbContext.SaveChanges();
        }
    }
}
