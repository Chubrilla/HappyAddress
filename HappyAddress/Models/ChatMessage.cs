using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HappyAddress.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }

        public int ChatId { get; set; }
        public int SenderUserId { get; set; }

        public string Text { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;

        [ValidateNever]
        public Chat? Chat { get; set; }

        [ValidateNever]
        public User? SenderUser { get; set; }
    }
}
