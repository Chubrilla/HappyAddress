using System.Collections.Generic;

namespace HappyAddress.Models
{
    public class ChatDetailsViewModel
    {
        public Chat Chat { get; set; }
        public Ad Ad { get; set; }
        public User CurrentUser { get; set; }
        public User OtherUser { get; set; }

        public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();

        public string? AdImagePath { get; set; }
        public int CurrentUserId { get; set; }
    }
}
