using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Reservations.DTO;

namespace ParkingHere.Application.Reservations.Queries
{
    public class GetParkingsPastReservations : IQuery<IEnumerable<ReservationDto>>
    {
        public Guid UserId { get; set; }
    }
}
