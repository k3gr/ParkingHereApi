using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ParkingHere.Application.Abstractions;
using ParkingHere.Application.Spots.Commands;
using ParkingHere.Application.Spots.DTO;
using ParkingHere.Application.Spots.Queries;

namespace ParkingHere.WebApi.Controllers
{
    [Route("api/parkings/{parkingId}/spots")]
    [ApiController]
    public class SpotsController : ControllerBase
    {
        private readonly IQueryHandler<GetAvailableSpotsByParams, IEnumerable<SpotDto>> _getAvailableSpotsByParamsHandler;
        private readonly IQueryHandler<GetSpot, SpotDto> _getSpotHandler;
        private readonly ICommandHandler<CreateSpot> _createSpotHandler;
        private readonly ICommandHandler<UpdateSpot> _updateSpotHandler;

        public SpotsController(
            IQueryHandler<GetAvailableSpotsByParams, IEnumerable<SpotDto>> getAvailableSpotsByParamsHandler,
            IQueryHandler<GetSpot, SpotDto> getSpotHandler,
            ICommandHandler<CreateSpot> createSpotHandler,
            ICommandHandler<UpdateSpot> updateSpotHandler)
        {
            _getAvailableSpotsByParamsHandler = getAvailableSpotsByParamsHandler;
            _getSpotHandler = getSpotHandler;
            _createSpotHandler = createSpotHandler;
            _updateSpotHandler = updateSpotHandler;
        }

        [HttpPost("available-spots")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get available spots")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SpotDto>>> GetAvailableSpotsByParams(Guid parkingId, GetAvailableSpotsByParams query)
        {
            query.ParkingId = parkingId;
            var spots = await _getAvailableSpotsByParamsHandler.HandleAsync(query);
            return Ok(spots);
        }

        [HttpGet("{spotId:guid}")]
        [SwaggerOperation(Summary = "Get a specific spot")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SpotDto>> Get(Guid spotId)
        {
            var spot = await _getSpotHandler.HandleAsync(new GetSpot { SpotId = spotId });
            if (spot is null)
            {
                return NotFound();
            }
            return Ok(spot);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new parking spot")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Create([FromRoute] Guid parkingId, CreateSpot command)
        {
            command = command with { SpotId = Guid.NewGuid(), ParkingId = parkingId };
            await _createSpotHandler.HandleAsync(command);
            return Created();        
        }

        [HttpPut("{spotId:guid}")]
        [SwaggerOperation(Summary = "Update a parking spot")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Put(Guid spotId, UpdateSpot command)
        {
            await _updateSpotHandler.HandleAsync(command with { SpotId = spotId });
            return NoContent();
        }
    }
}
