using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Vehicles.DTO;

namespace ParkingHere.Application.Vehicles.Queries;
public class GetVehicleByUser : IQuery<VehicleDto>
{
    public Guid UserId { get; set; }
}
