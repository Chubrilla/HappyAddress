using System;

namespace HappyAddress.Models
{
    public class ChatListItemViewModel
    {
        public Chat Chat { get; set; }
        public Ad Ad { get; set; }
        public User OtherUser { get; set; }

        public ChatMessage? LastMessage { get; set; }
        public int UnreadCount { get; set; }

        public string? AdImagePath { get; set; }
    }
}
