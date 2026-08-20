using System.Collections.Generic;
using MainProject.DTOs;
using MainProject.Models;

namespace MainProject.Services.Abstract
{
    public interface ITicketService
    {
        // Yeni talep oluşturma metodu 
        Ticket CreateTicket(CreateTicketDto dto, int userId);
    }
}