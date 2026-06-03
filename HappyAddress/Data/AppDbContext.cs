using Microsoft.EntityFrameworkCore;
using HappyAddress.Models;

namespace HappyAddress.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Ad> Ads { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<AdImage> AdImages { get; set; }

        public DbSet<UserRating> UserRatings { get; set; }
        public DbSet<UserSubscription> UserSubscriptions { get; set; }

        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
    }
}