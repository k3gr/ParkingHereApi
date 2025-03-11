using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Reservations.Commands;
using ParkingHere.Application.Reservations.DTO;
using ParkingHere.Application.Reservations.Queries;
using Swashbuckle.AspNetCore.Annotations;

namespace ParkingHere.WebApi.Controllers
{
    [Route("api/reservations")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IQueryHandler<GetReservationsBySpot, IEnumerable<ReservationDto>> _getReservationsBySpotHandler;
        private readonly IQueryHandler<GetMyCurrentReservations, IEnumerable<ReservationDto>> _getMyCurrentReservationHandler;
        private readonly IQueryHandler<GetMyPastReservations, IEnumerable<ReservationDto>> _getMyPastReservationHandler;
        private readonly IQueryHandler<GetParkingsCurrentReservations, IEnumerable<ReservationDto>> _getParkingsCurrentReservationHandler;
        private readonly IQueryHandler<GetParkingsPastReservations, IEnumerable<ReservationDto>> _getParkingsPastReservationHandler;
        private readonly ICommandHandler<CreateReservation> _createReservationHandler;

        public ReservationsController(IQueryHandler<GetReservationsBySpot, IEnumerable<ReservationDto>> getReservationsHandler,
            IQueryHandler<GetMyCurrentReservations, IEnumerable<ReservationDto>> getMyCurrentReservationHandler,
            IQueryHandler<GetMyPastReservations, IEnumerable<ReservationDto>> getMyPastReservationHandler,
            IQueryHandler<GetParkingsCurrentReservations, IEnumerable<ReservationDto>> getParkingsCurrentReservationHandler,
            IQueryHandler<GetParkingsPastReservations, IEnumerable<ReservationDto>> getParkingsPastReservationHandler,
            ICommandHandler<CreateReservation> createReservationHandler)
        {
            _getReservationsBySpotHandler = getReservationsHandler;
            _getMyCurrentReservationHandler = getMyCurrentReservationHandler;
            _getMyPastReservationHandler = getMyPastReservationHandler;
            _getParkingsCurrentReservationHandler = getParkingsCurrentReservationHandler;
            _getParkingsPastReservationHandler = getParkingsPastReservationHandler;
            _createReservationHandler = createReservationHandler;
        }

        [HttpGet("/spots/{spotId:guid}/reservations")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get reservations by spot ID")]
        [ProducesResponseType(typeof(IEnumerable<ReservationDto>), 200)]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservationsBySpot(Guid spotId)
            => Ok(await _getReservationsBySpotHandler.HandleAsync(new GetReservationsBySpot { SpotId = spotId }));

        [HttpPost("{spotId}/spots/reservation")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Create a new reservation")]
        [ProducesResponseType(201)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Create([FromRoute] Guid spotId, [FromBody] CreateReservation command)
        {
            if (User?.Identity?.IsAuthenticated != true || !Guid.TryParse(User.Identity.Name, out Guid userId))
                return Unauthorized("Invalid or missing user ID.");
            command = command with { ReservationId = Guid.NewGuid(), ParkingId = command.ParkingId, SpotId = spotId, UserId = userId };
            await _createReservationHandler.HandleAsync(command);
            return Created($"api/parking/{command.ParkingId}/spots/{command.SpotId}/reservation/{command.ReservationId}", null);
        }

        [HttpGet("my-current-reservations")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get current reservations of the user")]
        [ProducesResponseType(typeof(IEnumerable<ReservationDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetMyCurrentReservationAsync()
        {
            if (User?.Identity?.IsAuthenticated != true || !Guid.TryParse(User.Identity.Name, out Guid userId))
                return Unauthorized("Invalid or missing user ID.");
            var reservationDtos = await _getMyCurrentReservationHandler.HandleAsync(new GetMyCurrentReservations { UserId = userId });
            return Ok(reservationDtos);
        }

        [HttpGet("my-past-reservations")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get past reservations of the user")]
        [ProducesResponseType(typeof(IEnumerable<ReservationDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetMyPastReservationAsync()
        {
            if (User?.Identity?.IsAuthenticated != true || !Guid.TryParse(User.Identity.Name, out Guid userId))
                return Unauthorized("Invalid or missing user ID.");
            var reservationDtos = await _getMyPastReservationHandler.HandleAsync(new GetMyPastReservations { UserId = userId });
            return Ok(reservationDtos);
        }

        [HttpGet("parkings-current-reservations")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get all current reservations for user's parkings")]
        [ProducesResponseType(typeof(IEnumerable<ReservationDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetParkingsCurrentReservationAsync()
        {
            if (User?.Identity?.IsAuthenticated != true || !Guid.TryParse(User.Identity.Name, out Guid userId))
                return Unauthorized("Invalid or missing user ID.");
            var reservationDtos = await _getParkingsCurrentReservationHandler.HandleAsync(new GetParkingsCurrentReservations { UserId = userId });
            return Ok(reservationDtos);
        }

        [HttpGet("parkings-past-reservations")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get all past reservations for user's parkings")]
        [ProducesResponseType(typeof(IEnumerable<ReservationDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<IEnumerable<ReservationDto>>> GetParkingsPastReservationAsync()
        {
            if (User?.Identity?.IsAuthenticated != true || !Guid.TryParse(User.Identity.Name, out Guid userId))
                return Unauthorized("Invalid or missing user ID.");
            var reservationDtos = await _getParkingsPastReservationHandler.HandleAsync(new GetParkingsPastReservations { UserId = userId });
            return Ok(reservationDtos);
        }
    }
}
