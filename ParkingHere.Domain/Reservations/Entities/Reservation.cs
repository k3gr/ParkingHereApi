using ParkingHere.Domain.Parkings.Entities;
using ParkingHere.Domain.Spots.Entities;
using ParkingHere.Domain.Vehicles.Entities;

namespace ParkingHere.Domain.Reservations.Entities
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public Guid? ParkingId { get; set; }
        public virtual Parking Parking { get; set; }
        public Guid? SpotId { get; set; }
        public virtual Spot Spot { get; set; }
        public Guid? VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Guid? CreatedById { get; set; }
    }
}
