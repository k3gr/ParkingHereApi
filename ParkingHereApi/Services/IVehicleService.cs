using ParkingHereApi.Models;

namespace ParkingHereApi.Services
{
    public interface IVehicleService
    {
        void Update(int id, VehicleDto dto);
        VehicleDto GetById(int id);
    }
}
