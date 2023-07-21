using Microsoft.AspNetCore.Mvc;
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

        [HttpPost("register")]
        public ActionResult RegisterUser([FromBody] RegisterUserDto dto)
        {
            var id = _accountService.RegisterUser(dto);

            return Created($"/api/account/{id}", null);
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            var token = _accountService.GenerateJwt(dto);

            return Ok(token);
        }
    }
}
