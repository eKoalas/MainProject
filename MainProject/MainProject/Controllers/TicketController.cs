using System.Security.Claims;
using MainProject.DTOs;
using MainProject.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MainProject.Controllers
{
    [Authorize] //giriş yapmuş olanlar kullanır sadecee
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost("create")]
        public IActionResult Create(CreateTicketDto dto)
        {
            //  token içindeki Id bilgisini çek
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("Kullanıcı bilgisi token içerisinden alınamadı.");
            }

            int userId = int.Parse(userIdClaim.Value); 

            // Talebi oluştur
            var ticket = _ticketService.CreateTicket(dto, userId);

            return Ok(new { Message = "Talep başarıyla Draft olarak oluşturuldu!", TicketId = ticket.Id });
        }
    }
}