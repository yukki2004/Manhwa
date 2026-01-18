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
    public class UserLogConfiguration : IEntityTypeConfiguration<UserLog>
    {
        public void Configure(EntityTypeBuilder<UserLog> builder)
        {
            builder.ToTable("user_logs");
            builder.HasKey(ul => ul.UserLogId);
            builder.Property(ul => ul.UserLogId)
                   .HasColumnName("user_log_id");
            builder.Property(ul => ul.IpAddress)
                   .HasColumnName("ip_address")
                   .HasMaxLength(200);
            builder.Property(ul => ul.Action)
                   .HasColumnName("act")
                   .HasConversion<short>()
                   .IsRequired();
            builder.Property(ul => ul.UserAgent)
                   .HasColumnName("user_agent")
                   .HasColumnType("text")
                   .IsRequired();
            builder.Property(ul => ul.CreatedAt)
                   .HasColumnName("create_at")
                   .HasColumnType("timestamptz")
                   .HasDefaultValueSql("now()")
                   .ValueGeneratedOnAdd();
            builder.Property(ul => ul.UserId)
                   .HasColumnName("user_id")
                   .IsRequired();
            builder.HasOne(ul => ul.User)
                   .WithMany(u => u.UserLogs)
                   .HasForeignKey(ul => ul.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
