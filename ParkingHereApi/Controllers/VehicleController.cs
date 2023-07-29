using Microsoft.AspNetCore.Mvc;
using ParkingHereApi.Models;
using ParkingHereApi.Services;

namespace ParkingHereApi.Controllers
{
    [Route("api/vehicle/{userId}/")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpGet]
        public ActionResult Get([FromRoute] int userId)
        {
            var user = _vehicleService.GetById(userId);

            return Ok(user);
        }

        [HttpPut]
        public ActionResult Update([FromRoute] int userId, [FromBody] VehicleDto dto)
        {
            _vehicleService.Update(userId, dto);

            return Ok();
        }
    }
}
