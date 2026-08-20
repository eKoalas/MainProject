using System;
using MainProject.Data;
using MainProject.DTOs;
using MainProject.Models;
using MainProject.Services.Abstract;

namespace MainProject.Services.Concrete
{
    public class TicketService : ITicketService
    {
        private readonly AppDbContext _context;

        
        public TicketService(AppDbContext context)
        {
            _context = context;
        }

        public Ticket CreateTicket(CreateTicketDto dto, int userId)
        {
            var newTicket = new Ticket
            {
                Description = dto.Description,
                Type = dto.Type,
                Status = "Draft",                                           // ilk durum her zaman --Draft-- başla
                CreatedAt = DateTime.UtcNow,                                // şu anki zamanı kaydeder
                UserId = userId                                             // talebi oluşturan mağazanın Id bilgisi
            };

            // kaydet
            _context.Tickets.Add(newTicket);
            _context.SaveChanges();

            return newTicket;
        }
    }
}