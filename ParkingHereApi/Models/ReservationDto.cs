using ParkingHereApi.Entities;

namespace ParkingHereApi.Models
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public int ParkingId { get; set; }
        public int SpotId { get; set; }
        public int VehicleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ParkingName { get; set; }
        public string ParkingAddress { get; set; }
        public string VehicleDetails { get; set; }
    }
}
