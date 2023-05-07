using AutoMapper;
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
            var restaurant = _mapper.Map<Parking>(dto);
            _dbContext.Parkings.Add(restaurant);
            _dbContext.SaveChanges();

            return restaurant.Id;
        }
    }
}
