using Manhwa.Domain.Entities;
using Manhwa.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Table & Key
            builder.ToTable("users");

            builder.HasKey(u => u.UserId);

            builder.Property(u => u.UserId)
                   .HasColumnName("user_id");

            // Basic Info
            builder.Property(u => u.Username)
                   .HasColumnName("username")
                   .HasMaxLength(50)
                   .IsRequired();



            builder.Property(u => u.Email)
                   .HasColumnName("email")
                   .HasMaxLength(500)
                   .IsRequired();
            builder.Property(u => u.Description)
                   .HasColumnName("description")
                   .HasColumnType("text");

            builder.Property(u => u.PasswordHash)
                   .HasColumnName("password_hash")
                   .HasColumnType("text");

            builder.Property(u => u.Avatar)
                   .HasColumnName("avatar")
                   .HasColumnType("text");

            builder.Property(u => u.GoogleId)
                   .HasColumnName("google_id")
                   .HasColumnType("text");

            // Enum fields
            builder.Property(u => u.LoginType)
                   .HasColumnName("login_type")
                   .HasConversion<short>()
                   .HasDefaultValue(LoginType.Local)
                   .IsRequired();

            builder.Property(u => u.Role)
                   .HasColumnName("role")
                   .HasConversion<short>()
                   .HasDefaultValue(UserRole.User)
                   .IsRequired();

            // Level & Exp
            builder.Property(u => u.CurrentExp)
                   .HasColumnName("current_exp")
                   .HasDefaultValue(0)
                   .IsRequired();

            builder.Property(u => u.Level)
                   .HasColumnName("level")
                   .HasColumnType("smallint")
                   .HasDefaultValue(0)
                   .IsRequired();

            // Status
            builder.Property(u => u.IsActive)
                   .HasColumnName("is_active")
                   .HasDefaultValue(true)
                   .IsRequired();

            // Audit
            builder.Property(u => u.CreatedAt)
                   .HasColumnName("create_at")
                   .HasColumnType("timestamptz")
                   .HasDefaultValueSql("now()")
                   .ValueGeneratedOnAdd();

            builder.Property(u => u.UpdatedAt)
                   .HasColumnName("update_at")
                   .HasColumnType("timestamptz")
                   .HasDefaultValueSql("now()")
                   .ValueGeneratedOnAdd();

            // Relationships
            builder.HasMany(u => u.RefreshTokens)
                   .WithOne(rt => rt.User)
                   .HasForeignKey(rt => rt.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.UserLogs)
                   .WithOne(ul => ul.User)
                   .HasForeignKey(ul => ul.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.ExpLogs)
                   .WithOne(el => el.User)
                   .HasForeignKey(el => el.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u=> u.stories)
                .WithOne(s=> s.User)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasMany(u => u.UserFavorites)
                   .WithOne(uf => uf.User)
                   .HasForeignKey(uf => uf.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Ratings)
                   .WithOne(r => r.User)
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u=> u.ReadingHistories)
                .WithOne(rh => rh.User)
                .HasForeignKey(rh => rh.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.Comments)
                   .WithOne(c => c.User)
                   .HasForeignKey(c => c.UserId)
                   .OnDelete(DeleteBehavior.SetNull);
            
            builder.HasMany(u => u.SentNotifications)
                   .WithOne(n => n.Sender)
                   .HasForeignKey(n => n.SenderId)
                   .OnDelete(DeleteBehavior.NoAction);

             builder.HasMany(u => u.UserNotifications)
                   .WithOne(un => un.User)
                   .HasForeignKey(un => un.UserId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(u => u.Reports)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
