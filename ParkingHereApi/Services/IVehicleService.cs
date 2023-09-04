using ParkingHereApi.Models;

namespace ParkingHereApi.Services
{
    public interface IVehicleService
    {
        void Update(int userId, VehicleDto dto);
        VehicleDto GetById(int userId);
        VehicleDto GetMyVehicle();

    }
}
