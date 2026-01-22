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
    public class RatingConfiguration : IEntityTypeConfiguration<Rating>
    {
        public void Configure(EntityTypeBuilder<Rating> builder)
        {
            // Mapping tên bảng
            builder.ToTable("rating");

            // Thiết lập Composite Primary Key (story_id, user_id)
            builder.HasKey(r => new { r.StoryId, r.UserId });

            builder.Property(r => r.StoryId).HasColumnName("story_id");
            builder.Property(r => r.UserId).HasColumnName("user_id");

            builder.Property(r => r.Score)
                   .HasColumnName("score")
                   .HasDefaultValue(0);

            // Audit Fields
            builder.Property(r => r.CreatedAt)
                   .HasColumnName("create_at")
                   .HasColumnType("timestamptz")
                   .HasDefaultValueSql("now()")
                   .ValueGeneratedOnAdd();

            builder.Property(r => r.UpdatedAt)
                   .HasColumnName("update_at")
                   .HasColumnType("timestamptz")
                   .HasDefaultValueSql("now()")
                   .ValueGeneratedOnAdd();

            // Relationships
            builder.HasOne(r => r.User)
                   .WithMany(u => u.Ratings)
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Story)
                   .WithMany(s => s.Ratings)
                   .HasForeignKey(r => r.StoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
