using ParkingHere.Application.Abstractions;

namespace ParkingHere.Application.ApplicationUsers.Queries
{
    public class VerifyPasswordToken : IQuery<bool>
    {
        public string Token { get; set; }
    }
}
