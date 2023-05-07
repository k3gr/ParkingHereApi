using Microsoft.AspNetCore.Mvc;
using ParkingHereApi.Entities;
using ParkingHereApi.Models;
using ParkingHereApi.Services;

namespace ParkingHereApi.Controllers
{
    [Route("api/parking/{parkingId}/spot")]
    [ApiController]
    public class ParkingSpotController : ControllerBase
    {
        private readonly IParkingSpotService _parkingSpotService;

        public ParkingSpotController(IParkingSpotService parkingSpotService)
        {
            _parkingSpotService = parkingSpotService;
        }

        [HttpDelete]
        public ActionResult Delete([FromRoute] int parkingId)
        {
            _parkingSpotService.RemoveAll(parkingId);

            return NoContent();
        }

        [HttpDelete("{spotId}")]
        public ActionResult DeleteById([FromRoute] int parkingId, [FromRoute] int spotId)
        {
            _parkingSpotService.Delete(parkingId, spotId);

            return NoContent();
        }

        [HttpPost]
        public ActionResult Create([FromRoute] int parkingId, [FromBody] CreateParkingSpotDto dto)
        {
            var newSpotId = _parkingSpotService.Create(parkingId, dto);

            return Created($"api/parking/{parkingId}/spot/{newSpotId}", null);
        }

        [HttpGet("{spotId}")]
        public ActionResult<ParkingSpotDto> Get([FromRoute] int parkingId, [FromRoute] int spotId)
        {
            ParkingSpotDto spot = _parkingSpotService.GetById(parkingId, spotId);

            return Ok(spot);
        }

        [HttpGet]
        public ActionResult<List<ParkingSpotDto>> Get([FromRoute] int parkingId)
        {
            var result = _parkingSpotService.GetAll(parkingId);

            return Ok(result);
        }
    }
}
