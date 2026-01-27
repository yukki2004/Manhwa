using Manhwa.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manhwa.Domain.Enums.Chapter;

namespace Manhwa.Infrastructure.Persistence.Configurations
{
    public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
    {
        public void Configure(EntityTypeBuilder<Chapter> builder)
        {
            // Table & Primary Key
            builder.ToTable("chapters");
            builder.HasKey(c => c.ChapterId);

            builder.Property(c => c.ChapterId)
                   .HasColumnName("chapter_id");

            // Properties
            builder.Property(c => c.Title)
                   .HasColumnName("title")
                   .HasColumnType("text");

            builder.Property(c => c.Slug)
                    .HasColumnName("slug")
                    .IsRequired();

            builder.Property(c => c.ChapterNumber)
                .HasColumnName("chapter_number")
                .HasPrecision(10, 2)
                .IsRequired()
                .HasDefaultValue(0.0);

            // Cấu hình cột Status
            builder.Property(c => c.Status)
                   .HasColumnName("status")
                   .HasConversion<short>()
                   .HasDefaultValue(ChapterStatus.Published)
                   .IsRequired();

            builder.Property(c => c.TotalView)
                   .HasColumnName("total_view")
                   .HasDefaultValue(0)
                   .IsRequired();

            // Audit
            builder.Property(c => c.CreatedAt)
                   .HasColumnName("create_at")
                   .HasColumnType("timestamptz")
                   .HasDefaultValueSql("now()")
                   .ValueGeneratedOnAdd();

            builder.Property(c => c.UpdatedAt)
                   .HasColumnName("update_at")
                   .HasColumnType("timestamptz")
                   .HasDefaultValueSql("now()")
                   .ValueGeneratedOnAdd();

            // Relationship: One Story has Many Chapters
            builder.Property(c => c.StoryId)
                   .HasColumnName("story_id")
                   .IsRequired();

            builder.HasOne(c => c.Story)
                   .WithMany(s => s.Chapters)
                   .HasForeignKey(c => c.StoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.ChapterImages)
                   .WithOne(ci => ci.Chapter)
                   .HasForeignKey(ci => ci.ChapterId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.ReadingHistories)
                   .WithOne(rh => rh.Chapter)
                   .HasForeignKey(rh => rh.ChapterId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Comments)
                   .WithOne(cm => cm.Chapter)
                   .HasForeignKey(cm => cm.ChapterId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
