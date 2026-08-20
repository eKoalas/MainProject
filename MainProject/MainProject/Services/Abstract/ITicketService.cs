using System.Collections.Generic;
using MainProject.DTOs;
using MainProject.Models;

namespace MainProject.Services.Abstract
{
    public interface ITicketService
    {
        // Yeni talep oluşturma metodu 
        Ticket CreateTicket(CreateTicketDto dto, int userId);

        IEnumerable<Ticket> GetAllTickets(int userId, string role);

        bool UpdateStatus(int ticketId, string newStatus, int userId, string role);

        Comment AddComment(int ticketId, int userId, string text);

        TicketDetailDto GetTicketDetails(int ticketId, int userId, string role);

    }
}