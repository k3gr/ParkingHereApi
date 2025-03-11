using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Vehicles.Queries;
using ParkingHere.Application.Vehicles.Commands;
using ParkingHere.Application.Vehicles.DTO;

namespace ParkingHere.WebApi.Controllers
{
    [Route("api/vehicles")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        private readonly IQueryHandler<GetVehicleByUser, VehicleDto> _getVehicleByUserHandler;
        private readonly ICommandHandler<UpdateVehicle> _updateVehicleHandler;

        public VehiclesController(
            IQueryHandler<GetVehicleByUser, VehicleDto> getVehicleByUserHandler,
            ICommandHandler<UpdateVehicle> updateVehicleHandler)
        {
            _getVehicleByUserHandler = getVehicleByUserHandler;
            _updateVehicleHandler = updateVehicleHandler;
        }

        [HttpGet("{userId:guid}")]
        [SwaggerOperation(Summary = "Get vehicle by user ID", Description = "Get a vehicle by user.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VehicleDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VehicleDto>> Get(Guid userId)
        {
            var vehicle = await _getVehicleByUserHandler.HandleAsync(new GetVehicleByUser { UserId = userId });
            if (vehicle is null)
            {
                return NotFound();
            }
            return Ok(vehicle);
        }

        [HttpPut("{vehicleId:guid}")]
        [SwaggerOperation(Summary = "Update vehicle details", Description = "Update the details of a vehicle based on its ID.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update(Guid vehicleId, UpdateVehicle command)
        {
            await _updateVehicleHandler.HandleAsync(command with { UserId = vehicleId });
            return Ok();
        }
    }
}
