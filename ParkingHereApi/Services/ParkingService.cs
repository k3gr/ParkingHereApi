﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ParkingHereApi.Authorization;
using ParkingHereApi.Entities;
using ParkingHereApi.Enums;
using ParkingHereApi.Exceptions;
using ParkingHereApi.Models;

namespace ParkingHereApi.Services
{
    public class ParkingService : IParkingService
    {
        private readonly ParkingDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ParkingService> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public ParkingService(ParkingDbContext dbContext, IMapper mapper, ILogger<ParkingService> logger, IAuthorizationService authorizationService, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        public IEnumerable<ParkingDto> GetAll() 
        {
            var parkings = _dbContext
                .Parkings
                .Include(p => p.Address)
                .Include(p => p.Spots)
                .Include(r => r.Reservations)
                .ToList();

            var parkingsDtos = _mapper.Map<List<ParkingDto>>(parkings);

            return parkingsDtos;
        }
        public ParkingDto GetById(int id)
        {
            var parking = _dbContext
                .Parkings
                .Include(p => p.Address)
                .Include(p => p.Spots)
                .Include(p => p.Reservations)
                .FirstOrDefault(p => p.Id == id);

            if (parking is null)
            {
                throw new NotFoundException("Parking not found");
            }

            var result = _mapper.Map<ParkingDto>(parking);

            return result;
        }

        public int Create(CreateParkingDto dto)
        {
            var parking = _mapper.Map<Parking>(dto);
            parking.CreatedById = _userContextService.GetUserId;
            _dbContext.Parkings.Add(parking);
            _dbContext.SaveChanges();

            _logger.LogInformation($"Parking with id: {parking.Id} has been created");

            return parking.Id;
        }

        public void Update(int id, UpdateParkingDto dto)
        {
            var parking = _dbContext
                .Parkings
                .FirstOrDefault(p => p.Id == id);

            if (parking is null)
            {
                throw new NotFoundException("Parking not found");
            }

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, parking,
                 new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            parking.Name = dto.Name;
            parking.Description = dto.Description;
            parking.Type = dto.Type;
            parking.ContactNumber = dto.ContactNumber;
            parking.ContactEmail = dto.ContactEmail;

            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _logger.LogError($"Parking with id: {id} DELETE action invoked");

            var parking = _dbContext
                .Parkings
                .FirstOrDefault(p => p.Id == id);

            if (parking is null)
            {
                throw new NotFoundException("Parking not found");
            }

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, parking,
                new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            _dbContext.Parkings.Remove(parking);
            _dbContext.SaveChanges();
        }
    }
}
