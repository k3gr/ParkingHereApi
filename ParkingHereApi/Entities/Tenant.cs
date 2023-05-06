namespace ParkingHereApi.Entities
{
    public class Tenant
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public int RoleId { get; set; }
        public virtual Role Role { get; set; }

        public int VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set; }
    }
}
