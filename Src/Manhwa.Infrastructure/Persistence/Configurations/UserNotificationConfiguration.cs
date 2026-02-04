using Manhwa.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Persistence.Configurations
{
    public class UserNotificationConfiguration : IEntityTypeConfiguration<UserNotification>
    {
        public void Configure(EntityTypeBuilder<UserNotification> builder)
        {
            builder.ToTable("user_notifications");

            // Thiết lập Composite Primary Key (user_id, notification_id)
            builder.HasKey(un => new { un.UserId, un.NotificationId });

            builder.Property(un => un.UserId).HasColumnName("user_id");
            builder.Property(un => un.NotificationId).HasColumnName("notification_id");

            builder.Property(un => un.IsRead)
                   .HasColumnName("is_read")
                   .IsRequired();

            builder.Property(un => un.ReadAt)
                   .HasColumnName("read_at")
                   .HasColumnType("timestamptz")
                   .HasDefaultValueSql("now()");

            builder.Property(un => un.CreatedAt)
                    .HasColumnName("create_at")          
                    .HasColumnType("timestamptz")        
                    .HasDefaultValueSql("now()")         
                    .ValueGeneratedOnAdd();

            // Relationships
            builder.HasOne(un => un.User)
                   .WithMany(u => u.UserNotifications)
                   .HasForeignKey(un => un.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(un => un.Notification)
                   .WithMany(n => n.UserNotifications)
                   .HasForeignKey(un => un.NotificationId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
