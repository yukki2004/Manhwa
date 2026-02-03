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
    public class UserFavoriteConfiguration : IEntityTypeConfiguration<UserFavorite>
    {
        public void Configure(EntityTypeBuilder<UserFavorite> builder)
        {
            // Mapping tên bảng
            builder.ToTable("user_favorites");

            builder.HasKey(uf => new { uf.StoryId, uf.UserId });

            // Cấu hình các cột
            builder.Property(uf => uf.StoryId)
                   .HasColumnName("story_id");

            builder.Property(uf => uf.UserId)
                   .HasColumnName("user_id");

            builder.Property(f => f.CreatedAt)
               .HasColumnName("create_at")
               .HasColumnType("timestamptz")
               .HasDefaultValueSql("now()")
               .ValueGeneratedOnAdd(); 

            // Thiết lập quan hệ với User (FK: user_id)
            builder.HasOne(uf => uf.User)
                   .WithMany(u => u.UserFavorites)
                   .HasForeignKey(uf => uf.UserId)
                   .OnDelete(DeleteBehavior.Cascade); 

            builder.HasOne(uf => uf.Story)
                   .WithMany(s => s.UserFavorites)
                   .HasForeignKey(uf => uf.StoryId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
