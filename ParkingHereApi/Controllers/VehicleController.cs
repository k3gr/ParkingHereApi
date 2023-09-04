using Microsoft.AspNetCore.Mvc;
using ParkingHereApi.Models;
using ParkingHereApi.Services;

namespace ParkingHereApi.Controllers
{
    [Route("api/vehicle")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet("{userId}")]
        public ActionResult Get([FromRoute] int userId)
        {
            var user = _vehicleService.GetById(userId);

            return Ok(user);
        }

        [HttpGet("my-vehicle")]
        public ActionResult Get()
        {
            var user = _vehicleService.GetMyVehicle();

            return Ok(user);
        }

        [HttpPut("{userId}")]
        public ActionResult Update([FromRoute] int userId, [FromBody] VehicleDto dto)
        {
            _vehicleService.Update(userId, dto);

            return Ok();
        }
    }
}
