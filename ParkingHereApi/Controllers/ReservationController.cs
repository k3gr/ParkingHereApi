using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingHereApi.Models;
using ParkingHereApi.Services;

namespace ParkingHereApi.Controllers
{
    [Route("api/reservation/")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet("{parkingId}/spot/{spotId}/reservation")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<ReservationDto>> GetAll([FromRoute] int parkingId, [FromRoute] int spotId)
        {
            var reservationDtos = _reservationService.GetAll(parkingId, spotId);

            return Ok(reservationDtos);
        }

        [HttpGet("{parkingId}")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<ReservationDto>> GetByParkingId([FromRoute] int parkingId)
        {
            var reservationDtos = _reservationService.GetByParkingId(parkingId);

            return Ok(reservationDtos);
        }

        [HttpPost("{parkingId}/spot/reservation")]
        [AllowAnonymous]
        public ActionResult Create([FromRoute] int parkingId, [FromBody] CreateReservationDto createReservationDto)
        {
            var newReservation = _reservationService.Create(parkingId, createReservationDto);

            return Created($"api/parking/{parkingId}/spot/{newReservation.SpotId}/reservation/{newReservation.Id}", null);
        }

        [HttpGet("my-reservations")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<ReservationDto>> GetMyReservation()
        {
            var reservationDtos = _reservationService.GetMyReservation();

            return Ok(reservationDtos);
        }

        [HttpGet("my-past-reservations")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<ReservationDto>> GetMyPastReservation()
        {
            var reservationDtos = _reservationService.GetMyPastReservation();

            return Ok(reservationDtos);
        }
        
        [HttpGet("all-parkings-reservations")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<ReservationDto>> GetAllParkingsCurrentReservation()
        {
            var reservationDtos = _reservationService.GetAllParkingsCurrentReservation();

            return Ok(reservationDtos);
        }
        
        [HttpGet("all-parkings-past-reservations")]
        [AllowAnonymous]
        public ActionResult<IEnumerable<ReservationDto>> GetAllParkingsPastReservation()
        {
            var reservationDtos = _reservationService.GetAllParkingsPastReservation();

            return Ok(reservationDtos);
        }
    }
}
