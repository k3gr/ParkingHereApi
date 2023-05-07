using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ParkingHereApi.Entities;
using ParkingHereApi.Exceptions;
using ParkingHereApi.Models;

namespace ParkingHereApi.Services
{
    public class ParkingSpotService : IParkingSpotService
    {
        private readonly ParkingDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ParkingSpotService> _logger;

        public ParkingSpotService(ParkingDbContext context, IMapper mapper, ILogger<ParkingSpotService> logger)
        {
            _dbContext = context;
            _mapper = mapper;
            _logger = logger;
        }

        public int Create(int parkingId, CreateParkingSpotDto dto)
        {
            var parking = GetParkingById(parkingId);

            var spotEntity = _mapper.Map<ParkingSpot>(dto);

            spotEntity.ParkingId = parkingId;

            _dbContext.ParkingSpots.Add(spotEntity);
            _dbContext.SaveChanges();

            return spotEntity.Id;
        }

        public ParkingSpotDto GetById(int parkingId, int spotId)
        {
            var parking = GetParkingById(parkingId);

            var spot = _dbContext.ParkingSpots.FirstOrDefault(d => d.Id == spotId);

            if (spot is null || spot.ParkingId != parkingId)
            {
                throw new NotFoundException("Spot not found");
            }

            var spotDtos = _mapper.Map<ParkingSpotDto>(spot);

            return spotDtos;
        }

        public List<ParkingSpotDto> GetAll(int parkingId)
        {
            var parking = GetParkingById(parkingId);
            var spotDtos = _mapper.Map<List<ParkingSpotDto>>(parking.Spots);

            return spotDtos;
        }

        public void Delete(int parkingId, int spotId)
        {
            _logger.LogError($"Spot with id: {spotId} DELETE action invoked");

            var parking = GetParkingById(parkingId);

            var spot = _dbContext.ParkingSpots.FirstOrDefault(s => s.Id == spotId);

            if (spot is null || spot.ParkingId != parkingId)
            {
                throw new NotFoundException("Spot not found");
            }

            _dbContext.ParkingSpots.Remove(spot);
            _dbContext.SaveChanges();
        }

        public void RemoveAll(int parkingId)
        {
            var parking = GetParkingById(parkingId);

            _dbContext.RemoveRange(parking.Spots);
            _dbContext.SaveChanges();
        }

        private Parking GetParkingById(int parkingId)
        {
            var parking = _dbContext
                .Parkings
                .Include(r => r.Spots)
                .FirstOrDefault(r => r.Id == parkingId);

            if (parking is null)
            {
                throw new NotFoundException("Parking not found");
            }

            return parking;
        }
    }
}
