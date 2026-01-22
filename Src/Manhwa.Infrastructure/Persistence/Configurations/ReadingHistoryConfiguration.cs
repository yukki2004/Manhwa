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

            builder.HasKey(rh => new { rh.UserId, rh.StoryId });

            builder.Property(rh => rh.UserId).HasColumnName("user_id");
            builder.Property(rh => rh.StoryId).HasColumnName("story_id");
            builder.Property(rh => rh.ChapterId).HasColumnName("chapter_id");

            builder.Property(rh => rh.LastReadAt)
                   .HasColumnName("last_read_at")
                   .HasColumnType("timestamptz")
                   .HasDefaultValueSql("now()");

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
                   .WithMany() 
                   .HasForeignKey(rh => rh.ChapterId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
