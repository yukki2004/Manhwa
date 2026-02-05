using Manhwa.Domain.Entities;
using Manhwa.Domain.Enums.Report;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manhwa.Infrastructure.Persistence.Configurations
{
    public class ReportConfiguration : IEntityTypeConfiguration<Report>
    {
        public void Configure(EntityTypeBuilder<Report> builder)
        {
            builder.ToTable("reports");

            builder.HasKey(r => r.ReportId);
            builder.Property(r => r.ReportId)
                   .HasColumnName("report_id");

            builder.Property(r => r.Reason)
                   .HasColumnName("reason")
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(r => r.TargetType)
                   .HasColumnName("target_type")
                   .HasConversion<short>()
                   .IsRequired();

            builder.Property(r => r.TargetId)
                   .HasColumnName("target_id")
                   .IsRequired();

            builder.Property(r => r.Status)
                   .HasColumnName("status")
                   .HasConversion<short>()
                   .HasDefaultValue(ReportStatus.Pending);

            builder.Property(r => r.Metadata)
                   .HasColumnName("metadata")
                   .HasColumnType("jsonb");

            builder.Property(r => r.CreateAt)
                   .HasColumnName("create_at")
                   .HasColumnType("timestamptz")
                   .HasDefaultValueSql("now()")
                   .ValueGeneratedOnAdd();

            builder.Property(r => r.UserId)
                   .HasColumnName("user_id");

            builder.HasOne(r => r.User)
                   .WithMany(u=> u.Reports)
                   .HasForeignKey(r => r.UserId)
                   .OnDelete(DeleteBehavior.SetNull);

            
        }
    }
}
