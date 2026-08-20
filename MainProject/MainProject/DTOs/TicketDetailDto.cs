using System;
using System.Collections.Generic;

namespace MainProject.DTOs
{
    public class TicketDetailDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }

        // Talebin altındaki yorumlar ve tarihçe
        public List<CommentDto> Comments { get; set; }
        public List<HistoryDto> Histories { get; set; }
    }

    public class CommentDto
    {
        public string Username { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class HistoryDto
    {
        public string OldStatus { get; set; }
        public string NewStatus { get; set; }
        public string ChangedBy { get; set; }
        public DateTime ChangedAt { get; set; }
    }
}