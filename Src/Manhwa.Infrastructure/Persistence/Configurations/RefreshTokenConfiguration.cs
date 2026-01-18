using Manhwa.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Persistence.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            // Table & Key
            builder.ToTable("refresh_tokens");

            builder.HasKey(rt => rt.RefreshTokenId);

            builder.Property(rt => rt.RefreshTokenId)
                   .HasColumnName("refresh_token_id");

            // Token
            builder.Property(rt => rt.Token)
                   .HasColumnName("token")
                   .HasColumnType("text")
                   .IsRequired();

            builder.HasIndex(rt => rt.Token)
                   .IsUnique();

            // Status
            builder.Property(rt => rt.IsUsed)
                   .HasColumnName("is_used")
                   .HasDefaultValue(false)
                   .IsRequired();

            // Time
            builder.Property(rt => rt.ExpiresAt)
                   .HasColumnName("expries_at")
                   .HasColumnType("timestamptz")
                   .IsRequired();

            builder.Property(rt => rt.CreatedAt)
                   .HasColumnName("create_at")
                   .HasColumnType("timestamptz")
                   .HasDefaultValueSql("now()")
                   .ValueGeneratedOnAdd();

            // Relationship
            builder.Property(rt => rt.UserId)
                   .HasColumnName("user_id")
                   .IsRequired();

            builder.HasOne(rt => rt.User)
                   .WithMany(u => u.RefreshTokens)
                   .HasForeignKey(rt => rt.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
