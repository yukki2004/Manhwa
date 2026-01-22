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
    public class StoryCategoryConfiguration : IEntityTypeConfiguration<StoryCategory>
    {
        public void Configure(EntityTypeBuilder<StoryCategory> builder)
        {
            // Mapping tên bảng
            builder.ToTable("story_categories");

            // Thiết lập Composite Primary Key
            builder.HasKey(sc => new { sc.StoryId, sc.CategoryId });

            builder.Property(sc => sc.StoryId).HasColumnName("story_id");
            builder.Property(sc => sc.CategoryId).HasColumnName("category_id");

            // Quan hệ với Story
            builder.HasOne(sc => sc.Story)
                   .WithMany(s => s.StoryCategories)
                   .HasForeignKey(sc => sc.StoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Quan hệ với Category
            builder.HasOne(sc => sc.Category)
                   .WithMany(c => c.StoryCategories)
                   .HasForeignKey(sc => sc.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
