using ParkingHere.Domain.ApplicationUsers.Entities;
using ParkingHere.Domain.Reservations.Entities;
using ParkingHere.Domain.Spots.Entities;

namespace ParkingHere.Domain.Parkings.Entities
{
    public class Parking
    {
        public Parking()
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }
        public Guid? CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }

        public Guid AddressId { get; set; }
        public virtual Address Address { get; set; }

        public virtual List<Spot> Spots { get; set; }
        public virtual List<Reservation> Reservations { get; set; }
    }
}
