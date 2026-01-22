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
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("notifications");

            builder.HasKey(n => n.NotificationId);
            builder.Property(n => n.NotificationId).HasColumnName("notification_id");

            builder.Property(n => n.Title)
                   .HasColumnName("title")
                   .IsRequired();

            builder.Property(n => n.Content)
                   .HasColumnName("content")
                   .HasColumnType("text")
                   .IsRequired();

            builder.Property(n => n.Type)
                   .HasColumnName("type")
                   .HasConversion<short>() 
                   .IsRequired();

            builder.Property(n => n.CreatedAt)
                   .HasColumnName("create_at")
                   .HasColumnType("timestamptz")
                   .HasDefaultValueSql("now()");

            // Relationships
            builder.Property(n => n.SenderId).HasColumnName("sender_id");

            builder.HasOne(n => n.Sender)
                   .WithMany(u => u.SentNotifications)
                   .HasForeignKey(n => n.SenderId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(n => n.UserNotifications)
                     .WithOne(un => un.Notification)
                     .HasForeignKey(un => un.NotificationId)
                     .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
