using MainProject.DTOs;
using MainProject.Services.Abstract; // IAuthService'i kullanabilmek için
using Microsoft.AspNetCore.Mvc;

namespace MainProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequestDto req)
        {
            var token = _authService.Login(req.Username, req.Password);

            if (token == null)
            {
                return BadRequest("Kullanıcı adı veya şifre hatalı bro.");
            }

            return Ok(new { token = token });
        }
    }
}