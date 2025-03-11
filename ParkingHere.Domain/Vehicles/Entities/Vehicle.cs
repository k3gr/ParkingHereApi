namespace ParkingHere.Domain.Vehicles.Entities
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string RegistrationPlate { get; set; }
        public Guid? CreatedById { get; set; }
    }
}
