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

        public IEnumerable<SpotDto> GetAll(int parkingId, DateParamsDto dateParamsDto)
        {
            var parking = GetParkingById(parkingId);

            var spots = GetAvailableSpots(parking.Spots, dateParamsDto.StartDate, dateParamsDto.EndDate);
            var spotDtos = _mapper.Map<List<SpotDto>>(spots);

            return spotDtos;
        }

        public int GetFirstAvailableSpotByType(int parkingId, CreateReservationDto createReservationDto)
        {
            var parking = GetParkingById(parkingId);

            var spots = GetAvailableSpots(parking.Spots, createReservationDto.StartDate, createReservationDto.EndDate);
            var spot = spots.FirstOrDefault(s => s.IsAvailable && s.Type.Equals(createReservationDto.Type));

            if (spot is null || spot.ParkingId != parkingId)
            {
                throw new NotFoundException("Spot not found");
            }

            var spotDto = _mapper.Map<SpotDto>(spot);

            return spotDto.Id;
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

        public void Create(int parkingId, CreateSpotDto dto)
        {
            var parking = GetParkingById(parkingId);

            var spotEntity = _mapper.Map<Spot>(dto);

            spotEntity.ParkingId = parkingId;

            _dbContext.Spots.Add(spotEntity);
            _dbContext.SaveChanges();
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

        private bool IsAvailableForReservation(Spot spot, DateTime startDate, DateTime endDate)
        {
            if (spot.Reservations != null)
            {
                foreach (var reservation in spot.Reservations)
                {
                    if (startDate >= reservation.StartDate && startDate <= reservation.EndDate
                        || endDate >= reservation.StartDate && endDate <= reservation.EndDate)
                    {
                        return false;
                    }
                    if (startDate <= reservation.StartDate && endDate >= reservation.EndDate)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private bool IsReservationUpToDate(DateTime endDate)
        {
            if (endDate > DateTime.Today)
            {
                return true;
            }

            return false;
        }

        private List<Spot> GetAvailableSpots(List<Spot> spots, DateTime startDate, DateTime endDate)
        {
            var spotList = new List<Spot>();
            if (spots == null) return spotList;

            foreach (var spot in spots)
            {
                var reservationList = new List<Reservation>();

                if (spot.Reservations != null)
                {
                    foreach (var reservation in spot.Reservations)
                    {
                        if (IsReservationUpToDate(reservation.EndDate))
                        {
                            reservationList.Add(reservation);
                        }
                    }
                }
                spot.Reservations = reservationList;
                if (IsAvailableForReservation(spot, startDate, endDate))
                {
                    spot.IsAvailable = IsAvailableForReservation(spot, startDate, endDate);
                    spotList.Add(spot);
                }
            }
            return spotList;
        }
    }
}
