namespace ParkingHere.Application.Reservations.DTO
{
    public class ReservationDto
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ParkingAddress { get; set; }
        public string VehicleDetails { get; set; }
    }
}
