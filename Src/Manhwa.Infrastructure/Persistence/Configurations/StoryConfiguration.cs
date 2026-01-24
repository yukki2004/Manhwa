using Manhwa.Domain.Entities;
using Manhwa.Domain.Enums;
using Manhwa.Domain.Enums.Story;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Persistence.Configurations
{
    public class StoryConfiguration : IEntityTypeConfiguration<Story>
    {
        public void Configure(EntityTypeBuilder<Story> builder)
        {
            // Table & Primary Key
            builder.ToTable("stories");
            builder.HasKey(s => s.StoryId);

            builder.Property(s => s.StoryId)
                   .HasColumnName("story_id");

            // Properties Mapping
            builder.Property(s => s.Slug)
                   .HasColumnName("slug")
                   .IsRequired();

            builder.Property(s => s.ThumbnailUrl)
                   .HasColumnName("thumbnail_url")
                   .HasColumnType("text");

            builder.Property(s => s.Title)
                   .HasColumnName("title")
                   .IsRequired();

            builder.Property(s => s.Description)
                   .HasColumnName("description")
                   .HasColumnType("text")
                   .IsRequired();

            builder.Property(s => s.ReleaseYear)
                   .HasColumnName("release_year");

            builder.Property(s => s.Author)
                   .HasColumnName("author")
                   .HasMaxLength(100);
            builder.Property(s => s.AdminNote)
                .HasColumnName("admin_note")
                .HasColumnType("text");

            // Enum & Default Values
            builder.Property(s => s.Status)
                   .HasColumnName("status")
                   .HasConversion<short>()
                   .HasDefaultValue(StoryStatus.Ongoing);

            builder.Property(s => s.AdminLockStatus)
                   .HasColumnName("admin_lock_status")
                   .HasConversion<short>()
                   .HasDefaultValue(AdminLockStatus.Normal);

            builder.Property(s => s.IsHot)
                   .HasColumnName("is_hot")
                   .HasDefaultValue(false);

            builder.Property(s => s.IsPublish)
                   .HasColumnName("is_publish")
                   .HasConversion<short>()
                   .HasDefaultValue(StoryPublishStatus.Published);

            // Stats
            builder.Property(s => s.TotalView)
                   .HasColumnName("total_view")
                   .HasDefaultValue(0);

            builder.Property(s => s.RateSum)
                   .HasColumnName("rate_sum")
                   .HasDefaultValue(0);

            builder.Property(s => s.RateCount)
                   .HasColumnName("rate_count")
                   .HasDefaultValue(0);

            builder.Property(s => s.RateAvg)
                   .HasColumnName("rate_avg")
                   .HasColumnType("decimal(3,2)")
                   .HasDefaultValue(0.00m);

            // Audit
            builder.Property(s => s.CreatedAt)
                   .HasColumnName("create_at")
                   .HasColumnType("timestamptz")
                   .HasDefaultValueSql("now()")
                   .ValueGeneratedOnAdd();

            builder.Property(s => s.UpdatedAt)
                   .HasColumnName("update_at")
                   .HasColumnType("timestamptz")
                   .HasDefaultValueSql("now()")
                   .ValueGeneratedOnAdd();

            // Relationships
            builder.Property(s => s.UserId)
                   .HasColumnName("user_id");

            builder.HasOne(s => s.User)
                   .WithMany(u => u.stories)
                   .HasForeignKey(s => s.UserId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(s => s.Chapters)
                   .WithOne(c => c.Story)
                   .HasForeignKey(c => c.StoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.Ratings)
                     .WithOne(r => r.Story)
                     .HasForeignKey(r => r.StoryId)
                     .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.StoryCategories)
                   .WithOne(sc => sc.Story)
                   .HasForeignKey(sc => sc.StoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.UserFavorites)
                   .WithOne(uf => uf.Story)
                   .HasForeignKey(uf => uf.StoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.ReadingHistories)
                   .WithOne(rh => rh.Story)
                   .HasForeignKey(rh => rh.StoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.Comments)
                   .WithOne(c => c.Story)
                   .HasForeignKey(c => c.StoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
