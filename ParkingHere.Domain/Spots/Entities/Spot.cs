using ParkingHere.Domain.Parkings.Entities;
using ParkingHere.Domain.Reservations.Entities;

namespace ParkingHere.Domain.Spots.Entities
{
    public class Spot
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; }
        public bool IsAvailable { get; set; }
        public Guid ParkingId { get; set; }
        public virtual Parking Parking { get; set; }
        public virtual List<Reservation> Reservations { get; set; }
    }
}
