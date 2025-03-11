using ParkingHere.Domain.Parkings.Entities;
using ParkingHere.Domain.Spots.Entities;

namespace ParkingHere.Domain.Spots.Services
{
    public interface ISpotService
    {
        Guid GetFirstAvailableSpotByType(Parking parking, DateTime startDate, DateTime endDate, string type);
        List<Spot> GetAvailableSpots(List<Spot> spots, DateTime startDate, DateTime endDate);
    }
}
