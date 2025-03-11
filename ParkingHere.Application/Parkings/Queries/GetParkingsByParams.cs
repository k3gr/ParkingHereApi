using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Parkings.DTO;

namespace ParkingHere.Application.Parkings.Queries
{
    public class GetParkingsByParams : IQuery<IEnumerable<ParkingDto>>
    {
        public string City { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
