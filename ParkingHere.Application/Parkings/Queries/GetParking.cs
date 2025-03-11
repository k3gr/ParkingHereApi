using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Parkings.DTO;

namespace ParkingHere.Application.Parkings.Queries
{
    public class GetParking : IQuery<ParkingDto>
    {
        public Guid ParkingId { get; set; }
    }
}
