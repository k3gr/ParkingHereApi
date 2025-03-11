using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Reservations.DTO;
namespace ParkingHere.Application.Reservations.Queries
{
    public class GetMyCurrentReservations : IQuery<IEnumerable<ReservationDto>>
    {
        public Guid UserId { get; set; }
    }
}
