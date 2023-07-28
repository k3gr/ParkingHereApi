using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingHereApi.Models;
using ParkingHereApi.Services;

namespace ParkingHereApi.Controllers
{
    [Route("api/parking/{parkingId}/spot/")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet("{spotId}/reservation")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<ReservationDto>> GetAll([FromRoute] int parkingId, [FromRoute] int spotId)
        {
            var reservationDtos = _reservationService.GetAll(parkingId, spotId);

            return Ok(reservationDtos);
        }

        [HttpPost("reservation")]
        [AllowAnonymous]
        public ActionResult Create([FromRoute] int parkingId, [FromBody] CreateReservationDto createReservationDto)
        {
            var newReservation = _reservationService.Create(parkingId, createReservationDto);

            return Created($"api/parking/{parkingId}/spot/{newReservation.SpotId}/reservation/{newReservation.Id}", null);
        }
    }
}
