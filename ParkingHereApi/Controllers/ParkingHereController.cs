using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingHereApi.Models;
using ParkingHereApi.Services;

namespace ParkingHereApi.Controllers
{
    [Route("api/parking")]
    [ApiController]
    //[Authorize(Roles = "Admin,Owner")]
    public class ParkingHereController : ControllerBase
    {
        private readonly IParkingService _parkingService;

        public ParkingHereController(IParkingService parkingService)
        {
            _parkingService = parkingService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<ParkingDto>> GetAll()
        {
            var parkingsDtos = _parkingService.GetAll();

            return Ok(parkingsDtos);
        }

        [HttpGet("my-parkings")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<ParkingDto>> GetMyParkings()
        {
            var parkingsDtos = _parkingService.GetMyParkings();

            return Ok(parkingsDtos);
        }
        
        [HttpPost("city")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<ParkingDto>> GetByParams(ReservationParamsDto reservationParamsDto)
        {
            var parkingsDtos = _parkingService.GetByParams(reservationParamsDto);

            return Ok(parkingsDtos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<ParkingDto> Get([FromRoute] int id)
        {
            var parking = _parkingService.GetById(id);

            return Ok(parking);
        }

        [HttpPost]
        public ActionResult CreateParking([FromBody] CreateParkingDto dto)
        {
            var id = _parkingService.Create(dto);

            return Created($"/api/parking/{id}", null);
        }
        
        [HttpPut("{id}")]
        public ActionResult UpdateParking([FromBody] UpdateParkingDto dto, [FromRoute] int id)
        {
            _parkingService.Update(id, dto);

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _parkingService.Delete(id);

            return NoContent();
        }
    }
}