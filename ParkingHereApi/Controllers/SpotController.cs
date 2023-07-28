using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingHereApi.Entities;
using ParkingHereApi.Models;
using ParkingHereApi.Services;

namespace ParkingHereApi.Controllers
{
    [Route("api/parking/{parkingId}/spot")]
    [ApiController]
    //[Authorize(Roles = "Admin,Owner")]
    public class SpotController : ControllerBase
    {
        private readonly ISpotService _spotService;

        public SpotController(ISpotService spotService)
        {
            _spotService = spotService;
        }

        [HttpPost("getAll")]
        public ActionResult<List<SpotDto>> GetAll([FromRoute] int parkingId, [FromBody] DateParamsDto dateParamsDto)
        {
            var result = _spotService.GetAll(parkingId, dateParamsDto);

            return Ok(result);
        }

        [HttpGet("{spotId}")]
        [AllowAnonymous]
        public ActionResult<SpotDto> GetById([FromRoute] int parkingId, [FromRoute] int spotId)
        {
            SpotDto spot = _spotService.GetById(parkingId, spotId);

            return Ok(spot);
        }
        
        [HttpDelete]
        public ActionResult Delete([FromRoute] int parkingId)
        {
            _spotService.RemoveAll(parkingId);

            return NoContent();
        }

        [HttpDelete("{spotId}")]
        public ActionResult DeleteById([FromRoute] int parkingId, [FromRoute] int spotId)
        {
            _spotService.Delete(parkingId, spotId);

            return NoContent();
        }

        [HttpPost]
        public ActionResult Create([FromRoute] int parkingId, [FromBody] CreateSpotDto dto)
        {
            var newSpotId = _spotService.Create(parkingId, dto);

            return Created($"api/parking/{parkingId}/spot/{newSpotId}", null);
        }
    }
}
