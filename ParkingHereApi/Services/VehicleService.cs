﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ParkingHereApi.Authorization;
using ParkingHereApi.Entities;
using ParkingHereApi.Enums;
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
        private readonly IAccountService _accountService;

        public VehicleService(ParkingDbContext dbContext, IMapper mapper, IAuthorizationService authorizationService, IUserContextService userContextService, IAccountService accountService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
            _accountService = accountService;
        }

        public VehicleDto GetById(int id)
        {
            var vehicle = _dbContext
                .Vehicles
                .FirstOrDefault(u => u.Id == id);

            if (vehicle is null)
            {
                throw new NotFoundException("Vehicle not found");
            }

            var vehicleDto = _mapper.Map<VehicleDto>(vehicle);

            return vehicleDto;
        }

        public VehicleDto GetMyVehicle()
        {
            var vehicle = _dbContext
                .Vehicles
                .FirstOrDefault(u => u.CreatedById == _userContextService.GetUserId);

            if (vehicle is null)
            {
                throw new NotFoundException("Vehicle not found");
            }

            var vehicleDto = _mapper.Map<VehicleDto>(vehicle);

            return vehicleDto;
        }

        public void Update(int id, VehicleDto dto)
        {
            var vehicle = _dbContext
                .Vehicles
                .FirstOrDefault(u => u.Id == id);

            if (vehicle is null)
            {
                throw new NotFoundException("Vehicle not found");
            }

            //var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, vehicle,
            //     new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            //if (!authorizationResult.Succeeded)
            //{
            //    throw new ForbidException();
            //}

            vehicle.Brand = dto.Brand;
            vehicle.Model = dto.Model;
            vehicle.RegistrationPlate = dto.RegistrationPlate;

            _dbContext.SaveChanges();
        }
    }
}
