namespace MainProject.Models
{
    public class TicketHistory
    {


        public int Id { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; }

        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
         
        public int ChangedByUserId { get; set; }
        public User ChangedByUser { get; set; }
        public DateTime ChangedAt { get; set; }



    }
}