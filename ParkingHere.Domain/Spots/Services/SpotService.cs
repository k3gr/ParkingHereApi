using ParkingHere.Domain.Parkings.Entities;
using ParkingHere.Domain.Reservations.Entities;
using ParkingHere.Domain.Reservations.Policies;
using ParkingHere.Domain.Spots.Entities;
using ParkingHere.Domain.Spots.Exceptions;

namespace ParkingHere.Domain.Spots.Services
{
    public class SpotService : ISpotService
    {
        private readonly IReservationPolicies _policies;

        public SpotService(IReservationPolicies policies)
        {
            _policies = policies;
        }

        public Guid GetFirstAvailableSpotByType(Parking parking, DateTime startDate, DateTime endDate, string type)
        {
            var spots = GetAvailableSpots(parking.Spots, startDate, endDate);
            var spot = spots.FirstOrDefault(s => s.IsAvailable && s.Type.Equals(type));

            if (spot is null || spot.ParkingId != parking.Id)
            {
                throw new ParkingSpotNotAvailableException();
            }
            return spot.Id;
        }

        public List<Spot> GetAvailableSpots(List<Spot> spots, DateTime startDate, DateTime endDate)
        {
            var spotList = new List<Spot>();
            if (spots == null) return spotList;

            foreach (var spot in spots)
            {
                var reservationList = new List<Reservation>();

                if (spot.Reservations != null)
                {
                    foreach (var reservation in spot.Reservations)
                    {
                        if (_policies.IsReservationUpToDate(reservation.EndDate))
                        {
                            reservationList.Add(reservation);
                        }
                    }
                }
                spot.Reservations = reservationList;
                if (_policies.IsAvailableForReservation(spot, startDate, endDate))
                {
                    spot.IsAvailable = _policies.IsAvailableForReservation(spot, startDate, endDate);
                    spotList.Add(spot);
                }
            }
            return spotList;
        }
    }
}
