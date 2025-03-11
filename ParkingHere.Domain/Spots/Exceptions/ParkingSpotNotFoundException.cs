namespace ParkingHere.Domain.Spots.Exceptions
{
    public class ParkingSpotNotAvailableException : Exception
    {
        public ParkingSpotNotAvailableException() : base("Parking spot not found") { }
    }
}
