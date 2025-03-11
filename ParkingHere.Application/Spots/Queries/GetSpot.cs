using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Spots.DTO;

namespace ParkingHere.Application.Spots.Queries
{
    public class GetSpot : IQuery<SpotDto>
    {
        public Guid SpotId { get; set; }
    }
}
