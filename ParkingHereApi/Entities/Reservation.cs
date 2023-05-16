namespace ParkingHereApi.Entities
{
    public class Reservation
    {
        public int Id { get; set; }
        public int? ParkingId { get; set; }
        public virtual Parking Parking { get; set; }
        public int? SpotId { get; set; }
        public virtual Spot Spot { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
