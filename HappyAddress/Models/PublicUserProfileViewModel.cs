using System.Collections.Generic;

namespace HappyAddress.Models
{
    public class PublicUserProfileViewModel
    {
        public User User { get; set; }

        public List<Ad> Ads { get; set; } = new List<Ad>();

        public int PublishedAdsCount { get; set; }

        public double AverageRating { get; set; }

        public int RatingsCount { get; set; }

        public bool IsSubscribed { get; set; }

        public int SubscribersCount { get; set; }

        public int? CurrentUserRating { get; set; }

        public bool IsOwner { get; set; }
    }
}