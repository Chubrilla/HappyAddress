using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HappyAddress.Models
{
    public class Chat
    {
        public int Id { get; set; }

        public int AdId { get; set; }
        public int SellerUserId { get; set; }
        public int BuyerUserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [ValidateNever]
        public Ad? Ad { get; set; }

        [ValidateNever]
        public User? SellerUser { get; set; }

        [ValidateNever]
        public User? BuyerUser { get; set; }

        [ValidateNever]
        public List<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
    }
}
