using ParkingHere.Domain.Vehicles.Entities;

namespace ParkingHere.Domain.ApplicationUsers.Entities
{
    public class User
    {
        public User() { }

        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PasswordHash { get; set; }

        public Guid RoleId { get; set; }
        public Guid? VehicleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual Vehicle Vehicle { get; set; }

        public string? ActivationToken { get; set; }
        public DateTime? ActivationDate { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpires { get; set; }
    }
}
