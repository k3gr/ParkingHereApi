using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingHereApi.Models;
using ParkingHereApi.Services;

namespace ParkingHereApi.Controllers
{
    [Route("api/parking/{parkingId}/spot/{spotId}/reservation")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<ReservationDto>> GetAll([FromRoute] int parkingId, [FromRoute] int spotId)
        {
            var reservationDtos = _reservationService.GetAll(parkingId, spotId);

            return Ok(reservationDtos);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Create([FromRoute] int parkingId, [FromRoute] int spotId, [FromBody] CreateReservationDto dto)
        {
            var newReservationId = _reservationService.Create(parkingId, spotId, dto);

            return Created($"api/parking/{parkingId}/spot/{spotId}/reservation/{newReservationId}", null);
        }
    }
}
