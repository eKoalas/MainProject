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

        public IEnumerable<Ticket> GetAllTickets(int userId, string role)
        {
            //centreş user herkesi görür
            if (role == "CenterUser")
            {
                return _context.Tickets.ToList();
            }

            // store user kendikileri görür
            return _context.Tickets.Where(t => t.UserId == userId).ToList();
        }




        public bool UpdateStatus(int ticketId, string newStatus, int userId, string role)
        {
            var ticket = _context.Tickets.Find(ticketId);
            if (ticket == null) throw new Exception("Talep bulunamadı.");

            string oldStatus = ticket.Status;
            bool isValidTransition = false;

            // store user sadece kendine erişebilr
            if (role == "StoreUser")
            {
                if (ticket.UserId != userId)
                    throw new Exception("Sadece kendi mağazanıza ait talepleri güncelleyebilirsiniz.");

                if (oldStatus == "Draft" && newStatus == "Submitted")
                    isValidTransition = true;
            }
            // centrel user her yere ulaşır 
            else if (role == "CenterUser")
            {
                if (oldStatus == "Submitted" && newStatus == "InReview") isValidTransition = true;
                else if (oldStatus == "InReview" && (newStatus == "Approved" || newStatus == "Rejected")) isValidTransition = true;
                else if ((oldStatus == "Approved" || oldStatus == "Rejected") && newStatus == "Completed") isValidTransition = true;
            }

            // Geçersiz bir sıraysa veya yetkisi yoksa reddet
            if (!isValidTransition)
                throw new Exception($"Geçersiz işlem: {role} rolü, {oldStatus} durumundan {newStatus} durumuna geçiş yapamaz.");

             ticket.Status = newStatus;

            //kaydet
            var history = new TicketHistory
            {
                TicketId = ticket.Id,
                OldStatus = oldStatus,
                NewStatus = newStatus,
                ChangedByUserId = userId,
                ChangedAt = DateTime.UtcNow
            };
              
            _context.TicketHistories.Add(history);
            _context.SaveChanges();

            return true;
        }

        public Comment AddComment(int ticketId, int userId, string text)
        {
            //talep gerçekten var mı kontrol
            var ticket = _context.Tickets.Find(ticketId);
            if (ticket == null) throw new Exception("Talep bulunamadı.");

            //yorum oluştur
            var comment = new Comment
            {
                TicketId = ticketId,
                UserId = userId,
                Text = text,
                CreatedAt = DateTime.UtcNow // Zaman bilgisini anlık olarak al
            };

            // kaydet
            _context.Comments.Add(comment);
            _context.SaveChanges();

            return comment;
        }

        public TicketDetailDto GetTicketDetails(int ticketId, int userId, string role)
        {
            // yetki kontrolü StoreUser sadece kendi talebine bakar
            if (role == "StoreUser")
            {
                var isOwner = _context.Tickets.Any(t => t.Id == ticketId && t.UserId == userId);
                if (!isOwner) throw new Exception("Bu talebi görüntüleme yetkiniz yok.");
            }

            // talebi yorumları ve geçmişi tek bir DTO içinde birleştirip çek
            var ticketDetails = _context.Tickets
                .Where(t => t.Id == ticketId)
                .Select(t => new TicketDetailDto
                {
                    Id = t.Id,
                    Description = t.Description,
                    Type = t.Type,
                    Status = t.Status,
                    CreatedAt = t.CreatedAt,
                    Comments = _context.Comments
                        .Where(c => c.TicketId == t.Id)
                        .Select(c => new CommentDto
                        {
                            Username = c.User.Username, 
                            Text = c.Text,
                            CreatedAt = c.CreatedAt     
                        }).ToList(),
                    Histories = _context.TicketHistories
                        .Where(h => h.TicketId == t.Id)
                        .Select(h => new HistoryDto
                        {
                            OldStatus = h.OldStatus,
                            NewStatus = h.NewStatus,
                            ChangedBy = h.ChangedByUser.Username,
                            ChangedAt = h.ChangedAt
                        }).ToList()
                })
                .FirstOrDefault();

            if (ticketDetails == null) throw new Exception("Talep bulunamadı.");

            return ticketDetails;
        }



    }
}