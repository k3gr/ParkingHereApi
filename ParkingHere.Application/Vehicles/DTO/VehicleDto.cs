namespace ParkingHere.Application.Vehicles.DTO
{
    public class VehicleDto
    {
        public Guid Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string RegistrationPlate { get; set; }
        public Guid? CreatedById { get; set; }
    }
}
