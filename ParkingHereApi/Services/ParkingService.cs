using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ParkingHereApi.Entities;
using ParkingHereApi.Exceptions;
using ParkingHereApi.Models;

namespace ParkingHereApi.Services
{
    public class ParkingService : IParkingService
    {
        private readonly ParkingDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ParkingService> _logger;

        public ParkingService(ParkingDbContext dbContext, IMapper mapper, ILogger<ParkingService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public IEnumerable<ParkingDto> GetAll() 
        {
            var parkings = _dbContext
                .Parkings
                .Include(p => p.Address)
                .Include(p => p.Spots)
                .ToList();

            var parkingsDtos = _mapper.Map<List<ParkingDto>>(parkings);

            return parkingsDtos;
        }

        public int Create(CreateParkingDto dto)
        {
            var parking = _mapper.Map<Parking>(dto);
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

            _dbContext.Parkings.Remove(parking);
            _dbContext.SaveChanges();
        }

        public ParkingDto GetById(int id)
        {
            var parking = _dbContext
                .Parkings
                .Include(p => p.Address)
                .Include(p => p.Spots)
                .FirstOrDefault(p => p.Id == id);

            if (parking is null)
            {
                throw new NotFoundException("Parking not found");
            }

            var result = _mapper.Map<ParkingDto>(parking);

            return result;
        }
    }
}
