using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Reservations.DTO;

namespace ParkingHere.Application.Reservations.Queries
{
    public class GetReservationHandler : IQuery<ReservationDto>
    {
        public Guid ReservationId { get; set; }
    }
}
