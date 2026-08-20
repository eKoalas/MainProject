using MainProject.DTOs;
using MainProject.Services.Abstract; // IAuthService'i kullanabilmek için
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

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

        [HttpPost("register")]
        public IActionResult Register(RegisterRequestDto req)
        {
            var isRegistered = _authService.Register(req.Username, req.Password);

            if (!isRegistered)
            {
                return BadRequest("Bu kullanıcı adı zaten alınmış bro, başka dene.");
            }

            return Ok("Kayıt işlemi başarılı! Artık giriş yapabilirsin.");
        }

        [Authorize] 
        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var userList = _authService.GetAllUsers();
            return Ok(userList);
        }

    }
}