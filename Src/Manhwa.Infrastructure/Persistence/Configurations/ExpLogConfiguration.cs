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
    class ExpLogConfiguration : IEntityTypeConfiguration<ExpLog>
    {
        public void Configure(EntityTypeBuilder<ExpLog> builder)
        {
            builder.ToTable("exp_logs");

            builder.HasKey(el => el.ExpLogId);

            builder.Property(el => el.ExpLogId)
                   .HasColumnName("exp_log_id");

            builder.Property(el => el.Action)
                   .HasColumnName("act")
                   .HasConversion<short>()
                   .IsRequired();

            builder.Property(el => el.ExpAmount)
                   .HasColumnName("exp_mount")
                   .IsRequired();

            builder.Property(el => el.CreatedAt)
                   .HasColumnName("create_at")
                   .HasColumnType("timestamptz")
                   .HasDefaultValueSql("now()")
                   .ValueGeneratedOnAdd();

            builder.Property(el => el.UserId)
                   .HasColumnName("user_id")
                   .IsRequired();

            builder.HasOne(el => el.User)
                   .WithMany(u => u.ExpLogs)
                   .HasForeignKey(el => el.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
