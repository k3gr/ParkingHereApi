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

        public ReservationService(ParkingDbContext dbContext, IMapper mapper, ILogger<ParkingService> logger, ISpotService spotService, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _spotService = spotService;
            _userContextService = userContextService;
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

        public IEnumerable<ReservationDto> GetAllParkingsCurrentReservation()
        {

            var parkingsId = _dbContext
                .Parkings
                .Where(r => r.CreatedById == _userContextService.GetUserId)
                .Select(r => r.Id)
                .ToList();

            var reservationsDtos = new List<ReservationDto>();

            foreach (var parkingId in parkingsId)
            {
                var reservations = GetByParkingId(parkingId);
                reservationsDtos.AddRange(reservations);
            }
            var currentReservationsDtos = reservationsDtos.Where(r => r.EndDate >= DateTime.Today);

            return currentReservationsDtos;
        }

        public IEnumerable<ReservationDto> GetAllParkingsPastReservation()
        {

            var parkingsId = _dbContext
                .Parkings
                .Where(r => r.CreatedById == _userContextService.GetUserId)
                .Select(r => r.Id)
                .ToList();

            var reservationsDtos = new List<ReservationDto>();

            foreach (var parkingId in parkingsId)
            {
                var reservations = GetByParkingId(parkingId);
                reservationsDtos.AddRange(reservations);
            }
            var pastReservationsDtos = reservationsDtos.Where(r => r.EndDate < DateTime.Today);

            return pastReservationsDtos;
        }

        public IEnumerable<ReservationDto> GetByParkingId(int parkingId)
        {
            var reservations = _dbContext
                .Reservations
                .Include(r => r.Parking)
                .Include(r => r.Parking.Address)
                .Include(r => r.Vehicle)
                .Where(r => r.ParkingId == parkingId)
                //.Where(r => r.EndDate >= DateTime.Today)
                .ToList();

            var reservationsDtos = _mapper.Map<List<ReservationDto>>(reservations);

            return reservationsDtos;
        }

        public IEnumerable<ReservationDto> GetMyReservation()
        {
            var reservations = _dbContext
                .Reservations
                .Include(r => r.Parking)
                .Include(r => r.Parking.Address)
                .Include(r => r.Vehicle)
                .Where(r => r.CreatedById == _userContextService.GetUserId)
                .Where(r => r.EndDate >= DateTime.Today)
                .ToList();

            var reservationsDtos = _mapper.Map<List<ReservationDto>>(reservations);

            return reservationsDtos;
        }

        public IEnumerable<ReservationDto> GetMyPastReservation()
        {
            var reservations = _dbContext
                .Reservations
                .Include(r => r.Parking)
                .Include(r => r.Parking.Address)
                .Include(r => r.Vehicle)
                .Where(r => r.CreatedById == _userContextService.GetUserId)
                .Where(r => r.EndDate < DateTime.Today)
                .ToList();

            var reservationsDtos = _mapper.Map<List<ReservationDto>>(reservations);

            return reservationsDtos;
        }

        public Reservation Create(int parkingId, CreateReservationDto createReservationDto)
        {
            var spotId = _spotService.GetFirstAvailableSpotByType(parkingId, createReservationDto);
            var reservation = _mapper.Map<Reservation>(createReservationDto);

            var vehicle = _dbContext
                .Vehicles
                .FirstOrDefault(v => v.CreatedById == _userContextService.GetUserId);

            if (vehicle is null)
            {
                throw new NotFoundException("Vehicle not found");
            }

            reservation.ParkingId = parkingId;
            reservation.SpotId = spotId;
            reservation.VehicleId = vehicle.Id;
            reservation.CreatedById = _userContextService.GetUserId;

            _dbContext.Reservations.Add(reservation);
            _dbContext.SaveChanges();

            return reservation;
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
    }
}
