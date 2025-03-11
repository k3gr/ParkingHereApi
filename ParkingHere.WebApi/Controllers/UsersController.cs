using Microsoft.AspNetCore.Mvc;
using ParkingHere.Application.Abstractions;
using ParkingHere.Application.ApplicationUsers.Commands;
using ParkingHere.Application.ApplicationUsers.DTO;
using ParkingHere.Application.ApplicationUsers.Queries;
using ParkingHere.Application.Security;
using ParkingHere.Domain.ApplicationUsers.Repositories;
using Swashbuckle.AspNetCore.Annotations;

namespace ParkingHere.WebApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IQueryHandler<GetUser, UserDto> _getUserHandler;
        private readonly ICommandHandler<RegisterUser> _registerUserHandler;
        private readonly ICommandHandler<SignIn> _signInHandler;
        private readonly ICommandHandler<UpdateUser> _updateUserHandler;
        private readonly ICommandHandler<ActivateUser> _activateUserHandler;
        private readonly ICommandHandler<ResetPassword> _resetPasswordHandler;
        private readonly ICommandHandler<ForgotPassword> _forgotPasswordHandler;
        private readonly IQueryHandler<VerifyPasswordToken, bool> _verifyPasswordTokenHandler;
        private readonly IUserRepository _userRepository;
        private readonly ITokenStorage _tokenStorage;
        private readonly IAuthenticator _authenticator;

        public UsersController(
            IQueryHandler<GetUser, UserDto> getUserHandler,
            ICommandHandler<RegisterUser> registerUserHandler,
            ICommandHandler<SignIn> signInHandler,
            ICommandHandler<UpdateUser> updateUserHandler,
            ICommandHandler<ActivateUser> activateUserHandler,
            ICommandHandler<ResetPassword> resetPasswordHandler,
            ICommandHandler<ForgotPassword> forgotPasswordHandler,
            IQueryHandler<VerifyPasswordToken, bool> verifyPasswordTokenHandler,
            IUserRepository userRepository,
            ITokenStorage tokenStorage,
            IAuthenticator authenticator)
        {
            _getUserHandler = getUserHandler;
            _registerUserHandler = registerUserHandler;
            _signInHandler = signInHandler;
            _updateUserHandler = updateUserHandler;
            _activateUserHandler = activateUserHandler;
            _resetPasswordHandler = resetPasswordHandler;
            _forgotPasswordHandler = forgotPasswordHandler;
            _verifyPasswordTokenHandler = verifyPasswordTokenHandler;
            _userRepository = userRepository;
            _tokenStorage = tokenStorage;
            _authenticator = authenticator;
        }

        [HttpGet("{userId:guid}")]
        [SwaggerOperation("Get user by ID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> Get([FromRoute] Guid userId)
        {
            var user = await _getUserHandler.HandleAsync(new GetUser { UserId = userId });
            if (user is null)
            {
                return NotFound();
            }

            return user;
        }

        [HttpPut("{userId:guid}")]
        [SwaggerOperation("Update the user account")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Update([FromRoute] Guid userId, [FromBody] UpdateUser command)
        {
            await _updateUserHandler.HandleAsync(command with { UserId = userId });

            return Ok();
        }

        [HttpPost("register")]
        [SwaggerOperation("Create the user account")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post(RegisterUser command)
        {
            var jwt = _authenticator.CreateToken(command.UserId, "tenant");
            _tokenStorage.Set(jwt);
            command = command with { UserId = Guid.NewGuid(), VehicleId = Guid.NewGuid(), ActivationToken = jwt.AccessToken };

            await _registerUserHandler.HandleAsync(command);

            return CreatedAtAction(nameof(Get), new { command.UserId }, jwt.AccessToken);
        }

        [HttpPost("login")]
        [SwaggerOperation("Sign in the user.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserTokenDto>> Post(SignIn command)
        {
            await _signInHandler.HandleAsync(command);
            var jwt = _tokenStorage.Get();

            var user = await _userRepository.GetByEmailAsync(command.Email);
            return new UserTokenDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = jwt.AccessToken,
                Email = user.Email,
                Expires = jwt.Expires
            };
        }

        [HttpPost("/api/account/activation")]
        [SwaggerOperation("Activate user account")]
        public async Task<ActionResult> Activation([FromQuery] string token)
        {
            await _activateUserHandler.HandleAsync(new ActivateUser(token));

            return Ok();
        }

        [HttpPost("verify-password-token")]
        [SwaggerOperation("Verify password reset token")]
        public async Task<ActionResult> VerifyPasswordResetToken([FromQuery] string token)
        {
            await _verifyPasswordTokenHandler.HandleAsync(new VerifyPasswordToken { Token = token });

            return Ok();
        }

        [HttpPost("forgot-password")]
        [SwaggerOperation("Request password reset")]
        public async Task<ActionResult<string>> ForgotPassword([FromBody] ForgotPassword command)
        {
            await _forgotPasswordHandler.HandleAsync(command);

            return NoContent();
        }

        [HttpPost("reset-password")]
        [SwaggerOperation("Reset password using token")]
        public async Task<ActionResult> ResetPassword([FromQuery] string token, [FromBody] ResetPassword command)
        {
            await _resetPasswordHandler.HandleAsync(command with { Token = token });

            return Ok();
        }
    }
}
