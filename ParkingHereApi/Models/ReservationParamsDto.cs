using ParkingHereApi.Enums;

namespace ParkingHereApi.Models
{
    public class ReservationParamsDto
    {
        public string City { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        //public ParkingSpotType Type { get; set; }
    }
}
