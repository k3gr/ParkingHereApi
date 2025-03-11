using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Reservations.DTO;

namespace ParkingHere.Application.Reservations.Queries
{
    public class GetReservationsBySpot : IQuery<IEnumerable<ReservationDto>>
    {
        public Guid SpotId { get; set; }
    }
}
