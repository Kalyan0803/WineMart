using Microsoft.AspNetCore.Mvc;

using WineMart.Models;
using WineMart.Services;

namespace WineMart.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(
            [FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result =
                await _authService.RegisterAsync(model);

            if (!result.Success)
            {
                return BadRequest(new
                {
                    errors = result.Errors
                });
            }

            return Ok(new
            {
                message = "Customer registered successfully!"
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result =
                await _authService.LoginAsync(model);

            if (!result.Success)
            {
                return Unauthorized(new
                {
                    message = result.Message
                });
            }

            return Ok(new
            {
                token = result.Token,
                message = result.Message
            });
        }
    }
}