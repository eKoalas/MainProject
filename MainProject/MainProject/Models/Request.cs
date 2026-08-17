namespace MainProject.Models
{
    public class Request
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public int StoreUserId { get; set; }
        public User StoreUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();
    }
}
