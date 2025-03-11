using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Spots.DTO;

namespace ParkingHere.Application.Spots.Queries;

public class GetSpots : IQuery<IEnumerable<SpotDto>>
{
}
