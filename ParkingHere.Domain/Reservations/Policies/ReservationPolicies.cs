using ParkingHere.Domain.Spots.Entities;

namespace ParkingHere.Domain.Reservations.Policies
{
    internal sealed class ReservationPolicies : IReservationPolicies
    {
        public bool IsAvailableForReservation(Spot spot, DateTime startDate, DateTime endDate)
        {
            if (spot.Reservations != null)
            {
                foreach (var reservation in spot.Reservations)
                {
                    if (startDate >= reservation.StartDate && startDate <= reservation.EndDate
                        || endDate >= reservation.StartDate && endDate <= reservation.EndDate)
                    {
                        return false;
                    }
                    if (startDate <= reservation.StartDate && endDate >= reservation.EndDate)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool IsReservationUpToDate(DateTime endDate)
        {
            if (endDate > DateTime.Today)
            {
                return true;
            }

            return false;
        }
    }
}
