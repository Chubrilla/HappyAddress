using System;

namespace HappyAddress.Models
{
    public class UserRating
    {
        public int Id { get; set; }

        public int RatedUserId { get; set; }

        public int RaterUserId { get; set; }

        public int Value { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User RatedUser { get; set; }

        public User RaterUser { get; set; }
    }
}