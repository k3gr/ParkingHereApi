using ParkingHereApi.Models;

namespace ParkingHereApi.Services
{
    public interface ISpotService
    {
        int Create(int parkingId, CreateSpotDto dto);
        SpotDto GetById(int parkingId, int spotId);
        List<SpotDto> GetAll(int parkingId);
        void Delete(int parkingId, int spotId);
        void RemoveAll(int parkingId);
    }
}
