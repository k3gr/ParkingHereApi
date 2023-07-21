using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ParkingHereApi.Entities;
using ParkingHereApi.Exceptions;
using ParkingHereApi.Models;

namespace ParkingHereApi.Services
{
    public class SpotService : ISpotService
    {
        private readonly ParkingDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<SpotService> _logger;

        public SpotService(ParkingDbContext context, IMapper mapper, ILogger<SpotService> logger)
        {
            _dbContext = context;
            _mapper = mapper;
            _logger = logger;
        }
        public List<SpotDto> GetAll(int parkingId)
        {
            var parking = GetParkingById(parkingId);
            var spotDtos = _mapper.Map<List<SpotDto>>(parking.Spots
                .Where(s => s.IsAvailable));

            return spotDtos;
        }

        public SpotDto GetById(int parkingId, int spotId)
        {
            var parking = GetParkingById(parkingId);

            var spot = _dbContext
                .Spots
                .Include(s => s.Reservations)
                .FirstOrDefault(d => d.Id == spotId);

            if (spot is null || spot.ParkingId != parkingId)
            {
                throw new NotFoundException("Spot not found");
            }

            var spotDtos = _mapper.Map<SpotDto>(spot);

            return spotDtos;
        }

        public int Create(int parkingId, CreateSpotDto dto)
        {
            var parking = GetParkingById(parkingId);

            var spotEntity = _mapper.Map<Spot>(dto);

            spotEntity.ParkingId = parkingId;

            _dbContext.Spots.Add(spotEntity);
            _dbContext.SaveChanges();

            return spotEntity.Id;
        }

        public void Delete(int parkingId, int spotId)
        {
            _logger.LogError($"Spot with id: {spotId} DELETE action invoked");

            var parking = GetParkingById(parkingId);

            var spot = _dbContext.Spots.FirstOrDefault(s => s.Id == spotId);

            if (spot is null || spot.ParkingId != parkingId)
            {
                throw new NotFoundException("Spot not found");
            }

            _dbContext.Spots.Remove(spot);
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
                .Include(s => s.Spots)
                .Include(r => r.Reservations)
                .FirstOrDefault(r => r.Id == parkingId);

            if (parking is null)
            {
                throw new NotFoundException("Parking not found");
            }

            return parking;
        }
    }
}
