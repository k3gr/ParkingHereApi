using ParkingHere.Domain.Exceptions;

namespace ParkingHere.Application.Common.Exceptions
{
    public class InvalidCredentialsException : CustomException
    {
        public InvalidCredentialsException() : base("Invalid credentials.")
        {
        }
    }
}
