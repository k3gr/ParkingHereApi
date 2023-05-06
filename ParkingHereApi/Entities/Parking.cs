namespace ParkingHereApi.Entities
{
    public class Parking
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int Size { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }
        public int? CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }

        public int AddressId { get; set; }
        public virtual Address Address { get; set; }

        public virtual List<ParkingSpot> Spaces { get; set; }

    }
}
