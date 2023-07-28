using ParkingHereApi.Entities;

namespace ParkingHereApi.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string VehicleName { get; set; }
        public string VehicleRegistrationPlate { get; set; }
    }
}
