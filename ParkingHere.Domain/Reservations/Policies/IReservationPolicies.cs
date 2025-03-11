using ParkingHere.Domain.Spots.Entities;

namespace ParkingHere.Domain.Reservations.Policies
{
    public interface IReservationPolicies
    {
        bool IsAvailableForReservation(Spot spot, DateTime startDate, DateTime endDate);
        bool IsReservationUpToDate(DateTime endDate);
    }
}
