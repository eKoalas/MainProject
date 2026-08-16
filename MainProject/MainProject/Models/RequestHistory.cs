namespace MainProject.Models
{
    public class RequestHistory
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
        public int ChangedById { get; set; }
        public User ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}
