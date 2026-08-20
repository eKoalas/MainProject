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

        [HttpGet("list")]
        public IActionResult GetTickets()
        {
            var userIdC = User.FindFirst(ClaimTypes.NameIdentifier);
            var roleC = User.FindFirst(ClaimTypes.Role);


            if (userIdC == null || roleC == null)
                return Unauthorized("Kullanıcı bilgileri eksik.");

            int userId = int.Parse(userIdC.Value);
            string role = roleC.Value;



            var tickets = _ticketService.GetAllTickets(userId, role);
            return Ok(tickets);

        }

        [HttpPut("{id}/status")]
        public IActionResult UpdateStatus(int id, [FromBody] UpdateTicketStatusDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var roleClaim = User.FindFirst(ClaimTypes.Role);

                if (userIdClaim == null || roleClaim == null)
                    return Unauthorized("Kullanıcı bilgileri eksik.");


                int userId = int.Parse(userIdClaim.Value);
                string role = roleClaim.Value;



                _ticketService.UpdateStatus(id, dto.NewStatus, userId, role);


                return Ok(new { message = "Durum başarıyla güncellendi ve tarihçeye kaydedildi." });



            }
            catch (Exception ex)
            {
                 
                var realError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return BadRequest(new { error = realError });


            }
        }

        [HttpPost("{id}/comment")]
        public IActionResult AddComment(int id, [FromBody] CreateCommentDto dto)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                    return Unauthorized("Kullanıcı bilgileri eksik.");

                int userId = int.Parse(userIdClaim.Value);

                var comment = _ticketService.AddComment(id, userId, dto.Text);

                return Ok(new { message = "Yorum başarıyla eklendi.", commentId = comment.Id });


            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.InnerException });

            }
        }

        [HttpGet("{id}/details")]
        public IActionResult GetTicketDetails(int id)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                var roleClaim = User.FindFirst(ClaimTypes.Role);

                if (userIdClaim == null || roleClaim == null)
                    return Unauthorized("Kullanıcı bilgileri eksik.");

                int userId = int.Parse(userIdClaim.Value);
                string role = roleClaim.Value;

                var details = _ticketService.GetTicketDetails(id, userId, role);
                return Ok(details);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}








