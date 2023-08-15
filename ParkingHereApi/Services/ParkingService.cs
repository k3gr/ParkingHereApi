using AutoMapper;
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

            foreach (var parking in parkingsDtos)
            {
                //parking.Prices = GetPrices(parking.Id);
            }

            return parkingsDtos;
        }

        public IEnumerable<ParkingDto> GetByParams(ReservationParamsDto reservationParamsDto)
        {
            var parkings = _dbContext
                .Parkings
                .Include(p => p.Address)
                .Include(p => p.Spots)
                .Include(r => r.Reservations)
                .Where(p => p.Address.City.StartsWith(reservationParamsDto.City))
                .ToList();

            var spots = parkings.Select(p => p.Spots).ToList();

            var parkingsDtos = _mapper.Map<List<ParkingDto>>(parkings);
            var availableParkingsDtos = GetAvailableParkingsWithPrices(parkingsDtos, spots, reservationParamsDto);

            return availableParkingsDtos;
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

            var parkingDto = _mapper.Map<ParkingDto>(parking);
            parkingDto.Prices = GetPrices(parking.Spots);

            return parkingDto;
        }

        public IEnumerable<ParkingDto> GetMyParkings()
        {
            var parkings = _dbContext
                .Parkings
                .Include(p => p.Address)
                .Where(p => p.CreatedById == _userContextService.GetUserId)
                .ToList();

            var parkingsDtos = _mapper.Map<List<ParkingDto>>(parkings);

            return parkingsDtos;
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
                .Include (p => p.Address)
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
            parking.Address.Street = dto.Street;
            parking.Address.City = dto.City;
            parking.Address.PostalCode = dto.PostalCode;
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

        public List<decimal> GetPrices(List<Spot> spots)
        {
            var UniqueSpotsByType = spots.GroupBy(s => s.Type).Select(group => group.First());

            var prices = new List<decimal>();

            foreach (var spot in UniqueSpotsByType)
            {
                prices.Add(spot.Price);
            }

            prices.Sort();

            return prices;
        }

        private bool IsAvailableForReservation(Reservation reservation, ReservationParamsDto reservationParamsDto)
        {
            if (reservationParamsDto.StartDate >= reservation.StartDate && reservationParamsDto.StartDate <= reservation.EndDate
                || reservationParamsDto.EndDate >= reservation.StartDate && reservationParamsDto.EndDate <= reservation.EndDate)
            {
                return false;
            }
            if (reservationParamsDto.StartDate <= reservation.StartDate && reservationParamsDto.EndDate >= reservation.EndDate)
            {
                return false;
            }
            return true;
        }

        private bool IsReservationUpToDate(Reservation reservation)
        {
            if (reservation.EndDate > DateTime.Today)
            {
                return true;
            }

            return false;
        }

        private bool IsParkingAvailable(List<Spot> spots, ReservationParamsDto reservationParamsDto)
        {
            if (spots == null) return false;

            foreach (var spot in spots)
            {
                var reservationList = new List<Reservation>();

                if (spot.Reservations != null)
                {
                    foreach (var reservation in spot.Reservations)
                    {
                        if (IsReservationUpToDate(reservation))
                        {
                            if (IsAvailableForReservation(reservation, reservationParamsDto))
                            {
                                return true;
                            }
                        }
                    }
                }
                return true;
            }
            return false;
        }

        private IEnumerable<ParkingDto> GetAvailableParkingsWithPrices(List<ParkingDto> parkingsDtos, List<List<Spot>> spots, ReservationParamsDto reservationParamsDto)
        {
            var availableParkingsDtos = new List<ParkingDto>();

            for (int i = 0; i < parkingsDtos.Count; i++)
            {
                if (IsParkingAvailable(spots[i], reservationParamsDto))
                {
                    parkingsDtos[i].Prices = GetPrices(spots[i]);
                    availableParkingsDtos.Add(parkingsDtos[i]);
                }
            }

            return availableParkingsDtos;
        }
    }
}
