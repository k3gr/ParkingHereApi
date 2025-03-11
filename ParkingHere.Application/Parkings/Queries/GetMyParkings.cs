using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Parkings.DTO;

namespace ParkingHere.Application.Parkings.Queries
{
    public class GetMyParkings : IQuery<IEnumerable<ParkingDto>>
    {
        public Guid UserId { get; set; }
    }
}
