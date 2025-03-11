namespace ParkingHere.Domain.Parkings
{
    public class ParkingNotFoundException : Exception
    {
        public ParkingNotFoundException() : base("Parking not found") { }
    }
}
