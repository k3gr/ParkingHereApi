using ParkingHereApi.Models;

namespace ParkingHereApi.Services
{
    public interface IParkingSpotService
    {
        int Create(int parkingId, CreateParkingSpotDto dto);
        ParkingSpotDto GetById(int parkingId, int spotId);
        List<ParkingSpotDto> GetAll(int parkingId);
        void Delete(int parkingId, int spotId);
        void RemoveAll(int parkingId);
    }
}
