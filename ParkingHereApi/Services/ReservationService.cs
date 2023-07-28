using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingHereApi.Authorization;
using ParkingHereApi.Entities;
using ParkingHereApi.Enums;
using ParkingHereApi.Exceptions;
using ParkingHereApi.Models;

namespace ParkingHereApi.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ParkingDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ParkingService> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;
        private readonly ISpotService _spotService;

        public ReservationService(ParkingDbContext dbContext, IMapper mapper, ILogger<ParkingService> logger, ISpotService spotService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _spotService = spotService;
        }

        public IEnumerable<ReservationDto> GetAll(int parkingId, int spotId)
        {
            var spot = GetById(parkingId, spotId);

            var reservations = _dbContext
                .Reservations
                .Where(r => r.SpotId == spotId)
                .ToList();

            var reservationsDtos = _mapper.Map<List<ReservationDto>>(reservations);

            return reservationsDtos;
        }
        private Spot GetById(int parkingId, int spotId)
        {
            var parking = _dbContext
                .Parkings
                .Include(r => r.Spots)
                .FirstOrDefault(r => r.Id == parkingId);

            if (parking is null)
            {
                throw new NotFoundException("Parking not found");
            }

            var spot = parking
                .Spots
                .FirstOrDefault(spot => spot.Id == spotId);

            if (spot is null)
            {
                throw new NotFoundException("Spot not found");
            }

            return spot;
        }

        public Reservation Create(int parkingId, CreateReservationDto createReservationDto)
        {
            var spotId = _spotService.GetFirstAvailableSpotByType(parkingId, createReservationDto);
            var reservationEntity = _mapper.Map<Reservation>(createReservationDto);

            reservationEntity.ParkingId = parkingId;
            reservationEntity.SpotId = spotId;

            _dbContext.Reservations.Add(reservationEntity);
            _dbContext.SaveChanges();

            return reservationEntity;
        }

        public void Delete(int id)
        {
            _logger.LogError($"Reservation with id: {id} DELETE action invoked");

            var reservation = _dbContext
                .Parkings
                .FirstOrDefault(p => p.Id == id);

            if (reservation is null)
            {
                throw new NotFoundException("Reservation not found");
            }

            _dbContext.Parkings.Remove(reservation);
            _dbContext.SaveChanges();
        }
    }
}
