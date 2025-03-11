using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Parkings.Commands;
using ParkingHere.Application.Parkings.DTO;
using ParkingHere.Application.Parkings.Queries;
using ParkingHere.Application.Reservations.DTO;
using Swashbuckle.AspNetCore.Annotations;

namespace ParkingHere.WebApi.Controllers
{
    [Route("api/parkings")]
    [ApiController]
    public class ParkingsController : ControllerBase
    {
        private readonly IQueryHandler<GetParkings, IEnumerable<ParkingDto>> _getParkingsHandler;
        private readonly IQueryHandler<GetMyParkings, IEnumerable<ParkingDto>> _getMyParkingsHandler;
        private readonly IQueryHandler<GetParking, ParkingDto> _getParkingHandler;
        private readonly ICommandHandler<CreateParking> _createParkingHandler;
        private readonly ICommandHandler<UpdateParking> _updateParkingHandler;
        private readonly IQueryHandler<GetParkingsByParams, IEnumerable<ParkingDto>> _getParkingsByParamsHandler;
        private readonly ICommandHandler<DeleteParking> _deleteParkingHandler;

        public ParkingsController(IQueryHandler<GetParkings, IEnumerable<ParkingDto>> getParkingsHandler,
            IQueryHandler<GetMyParkings, IEnumerable<ParkingDto>> getMyParkingsHandler,
            IQueryHandler<GetParking, ParkingDto> getParkingHandler,
            ICommandHandler<CreateParking> createParkingHandler,
            ICommandHandler<UpdateParking> updateParkingHandler,
            IQueryHandler<GetParkingsByParams, IEnumerable<ParkingDto>> getParkingsByParamsHandler,
            ICommandHandler<DeleteParking> deleteParkingHandler)
        {
            _getParkingsHandler = getParkingsHandler;
            _getMyParkingsHandler = getMyParkingsHandler;
            _getParkingHandler = getParkingHandler;
            _createParkingHandler = createParkingHandler;
            _updateParkingHandler = updateParkingHandler;
            _getParkingsByParamsHandler = getParkingsByParamsHandler;
            _deleteParkingHandler = deleteParkingHandler;
        }

        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get all parkings")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ParkingDto>>> GetAll([FromQuery] GetParkings query)
            => Ok(await _getParkingsHandler.HandleAsync(query));

        [HttpGet("my-parkings")]
        [SwaggerOperation(Summary = "Get parkings for the current user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ParkingDto>>> GetMyParkings()
        {
            if (User?.Identity?.IsAuthenticated != true || !Guid.TryParse(User.Identity.Name, out Guid userId))
                return Unauthorized("Invalid or missing user ID.");

            var query = new GetMyParkings { UserId = userId };
            var parkings = await _getMyParkingsHandler.HandleAsync(query);
            return Ok(parkings);
        }

        [HttpPost("city")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get parkings by city and date range")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ParkingDto>>> GetByParams(ReservationParamsDto reservationParamsDto)
        {
            var query = new GetParkingsByParams
            {
                City = reservationParamsDto.City,
                StartDate = reservationParamsDto.StartDate,
                EndDate = reservationParamsDto.EndDate
            };
            var parkings = await _getParkingsByParamsHandler.HandleAsync(query);
            return Ok(parkings);
        }

        [HttpGet("{parkingId:guid}")]
        [SwaggerOperation(Summary = "Get a specific parking by ID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ParkingDto>> Get(Guid parkingId)
        {
            var parking = await _getParkingHandler.HandleAsync(new GetParking { ParkingId = parkingId });
            if (parking is null)
            {
                return NotFound();
            }
            return parking;
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new parking")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post(CreateParking command)
        {
            if (User?.Identity?.IsAuthenticated != true || !Guid.TryParse(User.Identity.Name, out Guid userId))
                return Unauthorized("Invalid or missing user ID.");
            command = command with { ParkingId = Guid.NewGuid(), UserId = userId };
            await _createParkingHandler.HandleAsync(command);
            return CreatedAtAction(nameof(Get), new { command.ParkingId }, null);
        }

        [HttpPut("{parkingId:guid}")]
        [SwaggerOperation(Summary = "Update an existing parking")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Put(Guid parkingId, UpdateParking command)
        {
            await _updateParkingHandler.HandleAsync(command with { ParkingId = parkingId });
            return NoContent();
        }

        [HttpDelete("{parkingId:guid}")]
        [SwaggerOperation(Summary = "Delete a parking by ID")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete([FromRoute] Guid parkingId)
        {
            var command = new DeleteParking { ParkingId = parkingId };
            await _deleteParkingHandler.HandleAsync(command);
            return NoContent();
        }
    }
}
