using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingHereApi.Models;
using ParkingHereApi.Services;

namespace ParkingHereApi.Controllers
{
    [Route("api/parking")]
    public class ParkingHereController : ControllerBase
    {
        private readonly IParkingService _parkingService;

        public ParkingHereController(IParkingService parkingService)
        {
            _parkingService = parkingService;
        }

        [HttpPost]
        public ActionResult CreateParking([FromBody] CreateParkingDto dto)
        {
            var id = _parkingService.Create(dto);

            return Created($"/api/parking/{id}", null);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ParkingDto>> GetAll()
        {
            var parkingsDtos = _parkingService.GetAll();

            return Ok(parkingsDtos);
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdateParkingDto dto, [FromRoute] int id)
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

        [HttpGet("{id}")]
        public ActionResult<ParkingDto> Get([FromRoute] int id)
        {
            var parking = _parkingService.GetById(id);

            return Ok(parking);
        }
    }
}