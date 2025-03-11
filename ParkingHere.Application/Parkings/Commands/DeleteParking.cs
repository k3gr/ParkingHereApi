using ParkingHere.Application.Abstractions;

namespace ParkingHere.Application.Parkings.Commands
{
    public class DeleteParking : ICommand
    {
        public Guid ParkingId { get; set; }
    }
}
