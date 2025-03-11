using ParkingHere.Application.Reservations.DTO;
using ParkingHere.Application.Spots.DTO;

namespace ParkingHere.Application.Parkings.DTO
{
    public class ParkingDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public List<decimal> Prices { get; set; }
        public List<SpotDto> Spots { get; set; }
        public List<ReservationDto> Reservations { get; set; }
    }
}
