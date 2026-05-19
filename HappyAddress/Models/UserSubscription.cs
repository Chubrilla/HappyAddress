using System;

namespace HappyAddress.Models
{
    public class UserSubscription
    {
        public int Id { get; set; }

        public int SubscriberUserId { get; set; }

        public int TargetUserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User SubscriberUser { get; set; }

        public User TargetUser { get; set; }
    }
}