using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ParkingHereApi.Entities;
using ParkingHereApi.Models;

namespace ParkingHereApi.Services
{
    public class ParkingService : IParkingService
    {
        private readonly ParkingDbContext _dbContext;
        private readonly IMapper _mapper;

        public ParkingService(ParkingDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
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

            return parking.Id;
        }

        public void Update(int id, UpdateParkingDto dto)
        {
            var parking = _dbContext
                .Parkings
                .FirstOrDefault(p => p.Id == id);

            parking.Name = dto.Name;
            parking.Description = dto.Description;
            parking.Type = dto.Type;
            parking.ContactNumber = dto.ContactNumber;
            parking.ContactEmail = dto.ContactEmail;

            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var parking = _dbContext
                .Parkings
                .FirstOrDefault(p => p.Id == id);

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

            var result = _mapper.Map<ParkingDto>(parking);

            return result;
        }
    }
}
