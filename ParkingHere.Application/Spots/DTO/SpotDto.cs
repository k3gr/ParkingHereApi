using ParkingHere.Application.Reservations.DTO;

namespace ParkingHere.Application.Spots.DTO
{
    public class SpotDto
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; }
        public bool IsAvailable { get; set; }
        public List<ReservationDto> Reservations { get; set; }
    }
}
