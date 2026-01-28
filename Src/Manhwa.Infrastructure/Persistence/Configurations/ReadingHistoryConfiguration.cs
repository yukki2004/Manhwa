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
    public class ReadingHistoryConfiguration : IEntityTypeConfiguration<ReadingHistory>
    {
        public void Configure(EntityTypeBuilder<ReadingHistory> builder)
        {
            // Table mapping
            builder.ToTable("reading_history");
            builder.HasKey(h => new { h.UserId, h.ChapterId });

            builder.Property(h => h.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(h => h.ChapterId)
                .HasColumnName("chapter_id")
                .IsRequired();

            builder.Property(h => h.StoryId)
                .HasColumnName("story_id")
                .IsRequired();

            builder.Property(h => h.LastReadAt)
                .HasColumnName("last_read_at")
                .HasColumnType("timestamptz")
                .HasDefaultValueSql("now()")
                .IsRequired();

            // Relationships
            builder.HasOne(rh => rh.User)
                   .WithMany(u => u.ReadingHistories)
                   .HasForeignKey(rh => rh.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rh => rh.Story)
                   .WithMany(s => s.ReadingHistories)
                   .HasForeignKey(rh => rh.StoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(rh => rh.Chapter)
                   .WithMany(c => c.ReadingHistories) 
                   .HasForeignKey(rh => rh.ChapterId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
