using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingHereApi.Entities;
using ParkingHereApi.Models;
using ParkingHereApi.Services;

namespace ParkingHereApi.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("{id}")]
        public ActionResult Get([FromRoute] int id)
        {
            var user = _accountService.GetById(id);

            return Ok(user);
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromRoute] int id, [FromBody] UpdateUserDto dto)
        {
            _accountService.Update(id, dto);

            return Ok();
        }

        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] RegisterUserDto dto)
        {
            var id = _accountService.RegisterUser(dto);

            return Created($"/api/account/{id}", null);
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            var token = _accountService.Login(dto);

            return Ok(token);
        }

        [HttpPost("activation")]
        public ActionResult Activation([FromQuery] string token)
        {
            _accountService.Activation(token);

            return Ok();
        }

        [HttpPost("verify-password-token")]
        public ActionResult VerifyPasswordResetToken([FromQuery] string token)
        {
            _accountService.VerifyPasswordResetToken(token);

            return Ok();
        }

        [HttpPost("forgot-password")]
        public ActionResult ForgotPassword([FromBody] UserResetPasswordStep1Dto userResetPasswordStep1Dto)
        {
            _accountService.ForgotPassword(userResetPasswordStep1Dto);

            return Ok();
        }

        [HttpPost("reset-password")]
        public ActionResult ResetPassword([FromQuery] string token, [FromBody] UserResetPasswordStep2Dto userResetPasswordStep2Dto)
        {
            _accountService.ResetPassword(token, userResetPasswordStep2Dto);

            return Ok();
        }
    }
}
