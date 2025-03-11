using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Parkings.DTO;

namespace ParkingHere.Application.Parkings.Queries;

public class GetParkings : IQuery<IEnumerable<ParkingDto>>
{
}
