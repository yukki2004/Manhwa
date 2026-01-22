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
    public class ChapterImageConfiguration : IEntityTypeConfiguration<ChapterImage>
    {
        public void Configure(EntityTypeBuilder<ChapterImage> builder)
        {
            // Mapping tên bảng
            builder.ToTable("chapter_images");

            // Primary Key
            builder.HasKey(ci => ci.ChapterImageId);

            builder.Property(ci => ci.ChapterImageId)
                   .HasColumnName("chapter_image_id");

            // Cấu hình các cột
            builder.Property(ci => ci.ImageUrl)
                   .HasColumnName("image_url")
                   .HasColumnType("text")
                   .IsRequired();

            builder.Property(ci => ci.OrderIndex)
                   .HasColumnName("order_index")
                   .IsRequired();

            // Thiết lập quan hệ 1-N: Một Chapter có nhiều ChapterImages
            builder.Property(ci => ci.ChapterId)
                   .HasColumnName("chapter_id");

            builder.HasOne(ci => ci.Chapter)
                   .WithMany(c => c.ChapterImages)
                   .HasForeignKey(ci => ci.ChapterId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
