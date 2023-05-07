using ParkingHereApi.Models;

namespace ParkingHereApi.Services
{
    public interface IParkingService
    {
        IEnumerable<ParkingDto> GetAll();
        int Create(CreateParkingDto dto);
    }
}
