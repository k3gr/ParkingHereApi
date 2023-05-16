using ParkingHereApi.Entities;

namespace ParkingHereApi.Models
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public int ParkingId { get; set; }
        public int SpotId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
