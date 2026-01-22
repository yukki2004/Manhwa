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
    public class LevelExpConfiguration : IEntityTypeConfiguration<LevelExp>
    {
        public void Configure(EntityTypeBuilder<LevelExp> builder)
        {
            builder.ToTable("level_exp");

            builder.HasKey(l => l.LevelExpId);
            builder.Property(l => l.LevelExpId)
                   .HasColumnName("level_exp_id")
                   .ValueGeneratedOnAdd();

            builder.Property(l => l.Level)
                   .HasColumnName("level")
                   .IsRequired();

            builder.Property(l => l.ExpValue)
                   .HasColumnName("exp_value")
                   .IsRequired();
        }
    }
}
