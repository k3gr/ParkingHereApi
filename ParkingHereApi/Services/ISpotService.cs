using ParkingHereApi.Models;

namespace ParkingHereApi.Services
{
    public interface ISpotService
    {
        void Create(int parkingId, CreateSpotDto dto);
        SpotDto GetById(int parkingId, int spotId);
        IEnumerable<SpotDto> GetAll(int parkingId, DateParamsDto dateParamsDto);
        int GetFirstAvailableSpotByType(int parkingId, CreateReservationDto createReservationDto);
        void Delete(int parkingId, int spotId);
        void RemoveAll(int parkingId);
    }
}
