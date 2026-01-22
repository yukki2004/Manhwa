using Manhwa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Domain.Entities.ExpLog> ExpLogs { get; set; }
        public DbSet<Domain.Entities.RefreshToken> RefreshTokens { get; set; }
        public DbSet<Domain.Entities.User> Users { get; set; }
        public DbSet<Domain.Entities.UserLog> UserLogs { get; set; }
        public DbSet<Domain.Entities.Story> Stories { get; set; }
        public DbSet<UserFavorite> UserFavorites { get; set; }
        public DbSet<Domain.Entities.Chapter> Chapters { get; set; }
        public DbSet<ReadingHistory> ReadingHistories { get; set; }
        public DbSet<ChapterImage> ChapterImages { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<StoryCategory> StoryCategories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<ExpAction> ExpActions { get; set; }
        public DbSet<LevelExp> LevelExps { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
