using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Spots.DTO;
namespace ParkingHere.Application.Spots.Queries
{
    public class GetAvailableSpotsByParams : IQuery<IEnumerable<SpotDto>>
    {
        public Guid ParkingId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
