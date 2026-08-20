namespace MainProject.Models
    {
        public class Ticket
        {
            public int Id { get; set; }
            public int UserId { get; set; }

            public string Description { get; set; }
            public string Type { get; set; }
            public string Status { get; set; }
            public DateTime CreatedAt { get; set; }
            
            
            
            public User User { get; set; }






        }
    }


