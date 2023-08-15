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

        [HttpGet("{id}")]
        public ActionResult Get([FromRoute] int id)
        {
            var user = _vehicleService.GetById(id);

            return Ok(user);
        }

        [HttpGet("my-vehicle")]
        public ActionResult Get()
        {
            var user = _vehicleService.GetMyVehicle();

            return Ok(user);
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] VehicleDto dto)
        {
            _vehicleService.Update(id, dto);

            return Ok();
        }
    }
}
