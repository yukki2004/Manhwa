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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // Table mapping
            builder.ToTable("categories");

            // Primary Key
            builder.HasKey(c => c.CategoryId);

            builder.Property(c => c.CategoryId)
                   .HasColumnName("category_id")
                   .ValueGeneratedOnAdd();

            // Properties
            builder.Property(c => c.Slug)
                   .HasColumnName("slug")
                   .IsRequired();

            builder.Property(c => c.Name)
                   .HasColumnName("name")
                   .IsRequired();

            builder.Property(c => c.Description)
                   .HasColumnName("description")
                   .HasColumnType("text");

            // Audit Field
            builder.Property(c => c.CreatedAt)
                   .HasColumnName("create_at")
                   .HasColumnType("timestamptz")
                   .HasDefaultValueSql("now()")
                   .ValueGeneratedOnAdd();
            // Relationships

            builder.HasMany(c => c.StoryCategories)
                   .WithOne(sc => sc.Category)
                   .HasForeignKey(sc => sc.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
