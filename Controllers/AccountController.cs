
using GestaoFinanca.Models;
using GestaoFinanca.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestaoFinanca.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AccountController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public AccountController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest request)
        {
            var result = await _jwtService.Authenticate(request);
            if (result is null)
            {
                return Unauthorized();
            }
            return Ok(result);

        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _jwtService.Register(request);
            
            if (result is null)
            {
                return BadRequest("User with this email already exists!");
            }

            return Ok(result);
        }
    }
}